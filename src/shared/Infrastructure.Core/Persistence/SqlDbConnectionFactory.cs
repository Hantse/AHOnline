using Infrastructure.Core.Interfaces;
using Microsoft.Extensions.Configuration;
using System;
using System.Data;
using System.Data.SqlClient;

namespace Infrastructure.Core.Persistence
{
    public class SqlDbConnectionFactory : IDatabaseConnectionFactory
    {
        private readonly SqlConnection sqlConnection;

        public SqlDbConnectionFactory()
        {
            this.sqlConnection = new SqlConnection(Environment.GetEnvironmentVariable("Database"));
        }

        public IDbConnection GetConnection()
        {
            return new SqlConnection(Environment.GetEnvironmentVariable("Database"));
        }
    }
}
