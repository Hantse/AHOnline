using Dapper;
using DapperExtensions;
using Infrastructure.Core.Interfaces;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Infrastructure.Core.Persistence
{
    public abstract class CoreRepository<T>
        where T : CoreEntity
    {
        protected IDatabaseConnectionFactory connectionFactory;
        protected ILogger<CoreRepository<T>> logger;

        protected CoreRepository(IDatabaseConnectionFactory connectionFactory, ILogger<CoreRepository<T>> logger)
        {
            this.connectionFactory = connectionFactory;
            this.logger = logger;
        }

        public abstract Task<T> QueryOneAsync(T query);
        public abstract Task<T> QueryOneByIdAsync(Guid id);
        public abstract Task<IEnumerable<T>> QueryMultipleAsync(T query);
        public abstract Task<IEnumerable<T>> QueryMultipleByIdAsync(Guid[] ids);
        public abstract Task<Guid?> InsertAsync(T entity);
        public abstract Task<int> InsertsAsync(T[] entities);
        public abstract Task<int> UpdateAsync(T entity);
        public abstract Task<int> UpdatesAsync(T[] entities);
        public abstract Task<int> DeleteAsync(T entity);
        public abstract Task<int> DeletesAsync(T[] entities);

        protected async Task<Guid?> CoreInsertAsync(T entity)
        {
            try
            {
                var connection = connectionFactory.GetConnection();
                return await connection.InsertAsync(entity);
            }
            catch (Exception e)
            {
                logger.LogError(e, $"Error on insert single entity. {typeof(T)}");
                return null;
            }
        }

        protected async Task<int> CoreInsertsAsync(string sql, T[] entities)
        {
            try
            {
                var connection = connectionFactory.GetConnection();
                return await connection.ExecuteAsync(sql, entities);
            }
            catch (Exception e)
            {
                logger.LogError(e, $"Error on insert multiple entity. {typeof(T)}");
                return 0;
            }
        }

        protected Task<int> CoreUpdateAsync(string sql, T entity)
        {
            try
            {
                var connection = connectionFactory.GetConnection();
                entity.UpdateAt = DateTime.UtcNow;
                return connection.ExecuteAsync(sql, entity);
            }
            catch (Exception e)
            {
                logger.LogError(e, $"Error on update single entity. {typeof(T)}");
                return Task.FromResult(0);
            }
        }

        protected Task<int> CoreUpdatesAsync(string sql, T[] entities)
        {
            try
            {
                var connection = connectionFactory.GetConnection();
                foreach (var entity in entities)
                    entity.UpdateAt = DateTime.UtcNow;

                return connection.ExecuteAsync(sql, entities);
            }
            catch (Exception e)
            {
                logger.LogError(e, $"Error on update multiple entity. {typeof(T)}");
                return Task.FromResult(0);
            }
        }

        public Task<int> CoreDeleteAsync(string sql, T entity)
        {
            try
            {
                var connection = connectionFactory.GetConnection();
                return connection.ExecuteAsync(sql, entity);
            }
            catch (Exception e)
            {
                logger.LogError(e, $"Error on delete single entity. {typeof(T)}");
                return Task.FromResult(0);
            }
        }

        public Task<int> CoreDeletesAsync(string sql, T[] entities)
        {
            try
            {
                var connection = connectionFactory.GetConnection();
                return connection.ExecuteAsync(sql, entities);
            }
            catch (Exception e)
            {
                logger.LogError(e, $"Error on delete multiple entity. {typeof(T)}");
                return Task.FromResult(0);
            }
        }
    }
}
