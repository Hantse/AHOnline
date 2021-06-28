using AHSync.Worker.Shared.Interfaces;
using Dapper;
using Infrastructure.Core.Entities;
using Infrastructure.Core.Interfaces;
using Infrastructure.Core.Persistence;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AHSync.Worker.Shared.Repositories
{
    public class OperationHistoryRepository : CoreRepository<OperationHistory>, IOperationHistoryRepository
    {
        public string SQL_SELECT_ALL = "SELECT * FROM OperationHistory;";
        public OperationHistoryRepository(IDatabaseConnectionFactory connectionFactory, ILogger<CoreRepository<OperationHistory>> logger) : base(connectionFactory, logger)
        {
        }

        public override Task<int> DeleteAsync(OperationHistory entity)
        {
            throw new NotImplementedException();
        }

        public override Task<int> DeletesAsync(OperationHistory[] entities)
        {
            throw new NotImplementedException();
        }

        public override Task<Guid?> InsertAsync(OperationHistory entity)
        {
            return CoreInsertAsync(entity);
        }

        public override Task<int> InsertsAsync(OperationHistory[] entities)
        {
            throw new NotImplementedException();
        }

        public override async Task<IEnumerable<OperationHistory>> QueryMultipleAsync(OperationHistory query)
        {
            var connection = connectionFactory.GetConnection();

            using (var multi = connection.QueryMultiple(SQL_SELECT_ALL, query))
            {
                return await multi.ReadAsync<OperationHistory>();
            }
        }

        public override Task<IEnumerable<OperationHistory>> QueryMultipleByIdAsync(Guid[] ids)
        {
            throw new NotImplementedException();
        }

        public override Task<OperationHistory> QueryOneAsync(OperationHistory query)
        {
            throw new NotImplementedException();
        }

        public override Task<OperationHistory> QueryOneByIdAsync(Guid id)
        {
            throw new NotImplementedException();
        }

        public override Task<int> UpdateAsync(OperationHistory entity)
        {
            throw new NotImplementedException();
        }

        public override Task<int> UpdatesAsync(OperationHistory[] entities)
        {
            throw new NotImplementedException();
        }
    }
}
