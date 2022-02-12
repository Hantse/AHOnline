using AHSync.Worker.Shared.Interfaces;
using Dapper;
using Infrastructure.Core.Entities;
using Infrastructure.Core.Interfaces;
using Infrastructure.Core.Persistence;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace AHSync.Worker.Shared.Repositories
{
    public class ItemRepository : CoreRepository<Item>, IItemRepository
    {
        public string SQL_SELECT_ALL = "SELECT * FROM [Item] WHERE DeleteAt IS NULL;";

        public ItemRepository(IDatabaseConnectionFactory connectionFactory, ILogger<CoreRepository<Item>> logger) : base(connectionFactory, logger)
        {
        }

        public override Task<int> DeleteAsync(Item entity)
        {
            throw new NotImplementedException();
        }

        public override Task<int> DeletesAsync(Item[] entities)
        {
            throw new NotImplementedException();
        }

        public override Task<Guid?> InsertAsync(Item entity)
        {
            throw new NotImplementedException();
        }

        public override Task<int> InsertsAsync(Item[] entities)
        {
            throw new NotImplementedException();
        }

        public override async Task<IEnumerable<Item>> QueryMultipleAsync(Item query)
        {
            var connection = connectionFactory.GetConnection();

            using (var multi = connection.QueryMultiple(SQL_SELECT_ALL, query))
            {
                return await multi.ReadAsync<Item>();
            }
        }

        public override Task<IEnumerable<Item>> QueryMultipleByIdAsync(Guid[] ids)
        {
            throw new NotImplementedException();
        }

        public override Task<Item> QueryOneAsync(Item query)
        {
            throw new NotImplementedException();
        }

        public override Task<Item> QueryOneByIdAsync(Guid id)
        {
            throw new NotImplementedException();
        }

        public override Task<int> UpdateAsync(Item entity)
        {
            throw new NotImplementedException();
        }

        public override Task<int> UpdatesAsync(Item[] entities)
        {
            throw new NotImplementedException();
        }
    }
}
