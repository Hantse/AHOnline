namespace Infrastructure.Core.Persistence
{
    public class ImplicitlyConvertedParameter
    {
        public ImplicitlyConvertedParameter(string column, string parameterDataType, string tableDataType, string value, string conversion, string stmtSimple)
        {
            Column = column;
            ParameterDataType = parameterDataType;
            TableDataType = tableDataType;
            Value = value;
            Conversion = conversion;
            StmtSimple = stmtSimple;
        }

        public string Column { get; }
        public string ParameterDataType { get; }
        public string TableDataType { get; }
        public string Value { get; }
        public string Conversion { get; }
        public string StmtSimple { get; }

        public override string ToString() =>
            $"{nameof(Column)}:{Column}; {nameof(TableDataType)}:{TableDataType}; {nameof(ParameterDataType)}:{ParameterDataType}";
    }
}
