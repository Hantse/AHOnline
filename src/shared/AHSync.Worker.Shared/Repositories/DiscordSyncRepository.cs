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
    public class DiscordSyncRepository : CoreRepository<DiscordSync>, IDiscordSyncRepository
    {
        private const string SQLQUERY_SELECT_SINGLE = "SELECT * FROM [DiscordSyncUser] WHERE UserId = @UserId AND DeleteAt IS NULL;";
        private const string SQLQUERY_INSERT = "INSERT INTO DiscordSyncUser(Username, Discriminator, UserId, CreateAt, CreateBy) VALUES(@Username, @Discriminator, @UserId, @CreateAt, @CreateBy)";


        public DiscordSyncRepository(IDatabaseConnectionFactory connectionFactory, ILogger<CoreRepository<DiscordSync>> logger) : base(connectionFactory, logger)
        {
        }

        public override Task<int> DeleteAsync(DiscordSync entity)
        {
            throw new NotImplementedException();
        }

        public override Task<int> DeletesAsync(DiscordSync[] entities)
        {
            throw new NotImplementedException();
        }

        public async Task<int> InsertSingleAsync(DiscordSync entity)
        {
            try
            {
                var connection = connectionFactory.GetConnection();
                return await connection.ExecuteAsync(SQLQUERY_INSERT, entity);
            }
            catch (Exception e)
            {
                logger.LogError(e, $"Error on insert single entity. {typeof(DiscordSync)}");
                return -1;
            }
        }

        public override Task<Guid?> InsertAsync(DiscordSync entity)
        {
            throw new NotImplementedException();
        }

        public override Task<int> InsertsAsync(DiscordSync[] entities)
        {
            throw new NotImplementedException();
        }

        public override Task<IEnumerable<DiscordSync>> QueryMultipleAsync(DiscordSync query)
        {
            throw new NotImplementedException();
        }

        public override Task<IEnumerable<DiscordSync>> QueryMultipleByIdAsync(Guid[] ids)
        {
            throw new NotImplementedException();
        }

        public override Task<DiscordSync> QueryOneAsync(DiscordSync query)
        {
            var connection = connectionFactory.GetConnection();
            return connection.QueryFirstOrDefaultAsync<DiscordSync>(SQLQUERY_SELECT_SINGLE, query);
        }

        public override Task<DiscordSync> QueryOneByIdAsync(Guid id)
        {
            throw new NotImplementedException();
        }

        public override Task<int> UpdateAsync(DiscordSync entity)
        {
            throw new NotImplementedException();
        }

        public override Task<int> UpdatesAsync(DiscordSync[] entities)
        {
            throw new NotImplementedException();
        }
    }
}
