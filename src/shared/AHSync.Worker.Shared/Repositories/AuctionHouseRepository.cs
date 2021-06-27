﻿using AHSync.Worker.Shared.Interfaces;
using Dapper;
using Infrastructure.Core.Entities;
using Infrastructure.Core.Interfaces;
using Infrastructure.Core.Persistence;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Z.Dapper.Plus;

namespace AHSync.Worker.Shared.Repositories
{
    public class AuctionHouseRepository : CoreRepository<Auction>, IAuctionHouseRepository
    {
        public string SQL_SELECT_REALM_AND_FACTION_QUERY = "SELECT * FROM Auction WHERE RealmName = @RealmName AND RealmFaction = @RealmFaction;";
        public string SQL_SELECT_ONE_BY_ID = "SELECT TOP(1) * FROM Auction WHERE AuctionId = @AuctionId;";
        public string SQL_INSERT_QUERY = "INSERT INTO Auction(AuctionId, RealmName, RealmFaction, ItemName, ItemId, ItemRand, ItemSeed, Bid, Buyout, Quantity, TimeLeft, CreateAt, CreateBy) " +
                                            "VALUES(@AuctionId, @RealmName, @RealmFaction, @ItemName, @ItemId, @ItemRand, @ItemSeed, @Bid, @Buyout, @Quantity, @TimeLeft, @CreateAt, @CreateBy);";
        public string SQL_UPDATE_QUERY = "UPDATE Auction SET Bid = @Bid, Buyout = @Buyout, Quantity = @Quantity, TimeLeft = @TimeLeft, UpdateAt = @UpdateAt, UpdateBy = @UpdateBy WHERE AuctionId = @AuctionId;";
        public string SQL_DELETE_QUERY = "UPDATE Auction SET DeleteAt = @DeleteAt, DeleteBy = @DeleteBy WHERE AuctionId = @AuctionId;";

        public AuctionHouseRepository(IDatabaseConnectionFactory connectionFactory, ILogger<CoreRepository<Auction>> logger)
            : base(connectionFactory, logger)
        {
        }

        public override Task<int> DeleteAsync(Auction entity)
        {
            return CoreUpdateAsync(SQL_DELETE_QUERY, entity);
        }

        public override Task<int> DeletesAsync(Auction[] entities)
        {
            return CoreUpdatesAsync(SQL_DELETE_QUERY, entities);
        }

        public override Task<Guid?> InsertAsync(Auction entity)
        {
            return CoreInsertAsync(entity);
        }

        public override Task<int> InsertsAsync(Auction[] entities)
        {
            var connection = connectionFactory.GetConnection();
            var inserts = connection.BulkInsert(entities);
            return Task.FromResult(inserts.Current.Count());
        }

        public override async Task<IEnumerable<Auction>> QueryMultipleAsync(Auction query)
        {
            var connection = connectionFactory.GetConnection();

            using (var multi = connection.QueryMultiple(SQL_SELECT_REALM_AND_FACTION_QUERY, query))
            {
                return await multi.ReadAsync<Auction>();
            }
        }

        public override Task<IEnumerable<Auction>> QueryMultipleByIdAsync(Guid[] ids)
        {
            throw new NotImplementedException();
        }

        public override Task<Auction> QueryOneAsync(Auction query)
        {
            var connection = connectionFactory.GetConnection();
            return connection.QueryFirstOrDefaultAsync<Auction>(SQL_SELECT_ONE_BY_ID, query);
        }

        public override Task<Auction> QueryOneByIdAsync(Guid id)
        {
            var connection = connectionFactory.GetConnection();
            return connection.QueryFirstOrDefaultAsync<Auction>(SQL_SELECT_ONE_BY_ID, new { AuctionId = id });
        }

        public override Task<int> UpdateAsync(Auction entity)
        {
            return CoreUpdateAsync(SQL_UPDATE_QUERY, entity);
        }

        public override Task<int> UpdatesAsync(Auction[] entities)
        {
            var connection = connectionFactory.GetConnection();
            var inserts = connection.BulkUpdate(entities);
            return Task.FromResult(inserts.Current.Count());
        }
    }
}
