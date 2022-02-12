using AHSync.Worker.Shared.Interfaces;
using Dapper;
using DapperExtensions;
using Infrastructure.Core.Entities;
using Infrastructure.Core.Interfaces;
using Infrastructure.Core.Persistence;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace AHSync.Worker.Shared.Repositories
{
    public class ProfileRepository : CoreRepository<Profile>, IProfileRepository
    {
        private const string SQLQUERY_SELECT_SINGLE = "SELECT * FROM [Profile] WHERE DeleteAt IS NULL;";
        private const string SQLQUERY_INSERT = "INSERT INTO [Profile](Username, Discriminator, UserId, Email, CustomerId, CreateAt, CreateBy) VALUES(@Username, @Discriminator, @UserId, @Email, @CustomerId, @CreateAt, @CreateBy)";
        
        public ProfileRepository(IDatabaseConnectionFactory connectionFactory, ILogger<CoreRepository<Profile>> logger) : base(connectionFactory, logger)
        {
        }

        public override Task<int> DeleteAsync(Profile entity)
        {
            throw new NotImplementedException();
        }

        public override Task<int> DeletesAsync(Profile[] entities)
        {
            throw new NotImplementedException();
        }

        public async Task<int> InsertSingleAsync(Profile entity)
        {
            try
            {
                var connection = connectionFactory.GetConnection();
                return await connection.ExecuteAsync(SQLQUERY_INSERT, entity);
            }
            catch (Exception e)
            {
                logger.LogError(e, $"Error on insert single entity. {typeof(Profile)}");
                return -1;
            }
        }

        public override Task<Guid?> InsertAsync(Profile entity)
        {
            throw new NotImplementedException();
        }

        public override Task<int> InsertsAsync(Profile[] entities)
        {
            throw new NotImplementedException();
        }

        public override Task<IEnumerable<Profile>> QueryMultipleAsync(Profile query)
        {
            throw new NotImplementedException();
        }

        public override Task<IEnumerable<Profile>> QueryMultipleByIdAsync(Guid[] ids)
        {
            throw new NotImplementedException();
        }

        public override Task<Profile> QueryOneAsync(Profile query)
        {
            var connection = connectionFactory.GetConnection();
            return connection.QueryFirstOrDefaultAsync<Profile>(SQLQUERY_SELECT_SINGLE, query);
        }

        public override Task<Profile> QueryOneByIdAsync(Guid id)
        {
            throw new NotImplementedException();
        }

        public override Task<int> UpdateAsync(Profile entity)
        {
            throw new NotImplementedException();
        }

        public override Task<int> UpdatesAsync(Profile[] entities)
        {
            throw new NotImplementedException();
        }
    }
}
