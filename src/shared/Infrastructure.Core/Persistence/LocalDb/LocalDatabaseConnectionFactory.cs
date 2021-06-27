using Infrastructure.Core.Interfaces;
using System.Data;
using System.Data.SqlClient;

namespace Infrastructure.Core.Persistence
{
    public class LocalDatabaseConnectionFactory : IDatabaseConnectionFactory
    {
        private readonly SqlConnection sqlConnection;

        public LocalDatabaseConnectionFactory(string connectionString)
        {
            this.sqlConnection = new SqlConnection(connectionString);
        }

        public IDbConnection GetConnection()
        {
            return sqlConnection;
        }
    }
}
