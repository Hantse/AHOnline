using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml.Linq;

namespace Infrastructure.Core.Persistence
{
    public class QueryWithImplicitConvertion
    {
        public QueryWithImplicitConvertion(string statement, IEnumerable<ImplicitlyConvertedParameter> parameters)
        {
            Statement = statement;
            Parameters = parameters;
        }

        public string Statement { get; }

        public IEnumerable<ImplicitlyConvertedParameter> Parameters { get; }

        public override string ToString()
        {
            var stringBuilder = new StringBuilder();
            stringBuilder.AppendLine("Implicit Convertion detected in following statement: ");
            stringBuilder.AppendLine(Statement);

            foreach (var param in Parameters)
                stringBuilder.AppendLine($"[{param.Column}] Convertion from {param.ParameterDataType} to {param.TableDataType}");

            return stringBuilder.ToString();
        }

        public static QueryWithImplicitConvertion FromQueryPlan(string queryPlan)
        {
            var xmlDoc = XDocument.Parse(queryPlan);
            var ns = xmlDoc.Root.GetDefaultNamespace();
            var implicitConversions =
                (from stmtSimple in xmlDoc.Descendants(ns + "StmtSimple")
                 from scalarOperator in stmtSimple.Descendants(ns + "RelOp").Descendants(ns + "DefinedValues").Elements(ns + "DefinedValue").Elements(ns + "ScalarOperator")
                 let convertNode = scalarOperator.Element(ns + "Convert")
                 where convertNode != null && scalarOperator.Attribute("ScalarString").Value.StartsWith("CONVERT_IMPLICIT") == true
                 let conversion = scalarOperator.Attribute("ScalarString")
                 let tableDataType = scalarOperator.Element(ns + "Convert").Attribute("DataType").Value
                 where (tableDataType == "varchar" || tableDataType == "nvarchar") && scalarOperator.Element(ns + "Convert").Attribute("Implicit").Value == "1"
                 let columnRef = scalarOperator.Descendants(ns + "ColumnReference").Single().Attribute("Column").Value
                 join column in xmlDoc.Descendants(ns + "ParameterList").Descendants(ns + "ColumnReference")
                     on columnRef equals column.Attribute("Column").Value
                 let parameterDataType = column.Attribute("ParameterDataType").Value
                 let parameterValue = column.Attribute("ParameterCompiledValue").Value
                 where (tableDataType.StartsWith("nvarchar") && parameterDataType.StartsWith("varchar"))
                     || (tableDataType.StartsWith("varchar") && parameterDataType.StartsWith("nvarchar"))
                 select new
                 {
                     Statement = stmtSimple.Attribute("StatementText").Value,
                     Column = columnRef,
                     ParameterDataType = parameterDataType,
                     TableDataType = tableDataType,
                     Value = parameterValue,
                     Conversion = conversion.Value,
                     StmtSimple = stmtSimple.ToString()
                 })
                .ToArray();

            var implicitlyConvertedParameters =
                implicitConversions
                    .Select(ic => new ImplicitlyConvertedParameter(ic.Column, ic.ParameterDataType, ic.TableDataType, ic.Value, ic.Conversion, ic.StmtSimple))
                    .ToArray();

            if (implicitlyConvertedParameters.Length == 0)
                return null;

            return new QueryWithImplicitConvertion(implicitConversions.First().Statement, implicitlyConvertedParameters);
        }
    }
}
