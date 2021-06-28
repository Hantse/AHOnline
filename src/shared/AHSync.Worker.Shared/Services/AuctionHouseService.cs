using AHSync.Worker.Shared.Interfaces;
using Infrastructure.Core.Entities;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;

namespace AHSync.Worker.Shared.Services
{
    [Hangfire.Queue("ah-sync")]
    public class AuctionHouseService : IAuctionHouseService
    {
        private readonly IAuctionHouseRepository auctionHouseRepository;
        private readonly IWoWApiService woWApiService;
        private readonly ILogger<AuctionHouseService> logger;

        public AuctionHouseService(IAuctionHouseRepository auctionHouseRepository, ILogger<AuctionHouseService> logger, IWoWApiService woWApiService)
        {
            this.auctionHouseRepository = auctionHouseRepository;
            this.logger = logger;
            this.woWApiService = woWApiService;
        }

        [Hangfire.Queue("ah-sync")]
        public async Task<(bool success, int insertResult, int updateResult, int deleteResult)> TryProcessAsync(int realmId, string realmName, int realmFaction)
        {
            var auctionsToInsert = new List<Auction>();
            var auctionsToUpdate = new List<Auction>();
            var auctionsToDelete = new List<Auction>();

            Stopwatch sc = new Stopwatch();
            sc.Start();

            logger.LogInformation($"Data receive : realmId={realmId} - realmName={realmName} - realmFaction={realmFaction}");
            logger.LogInformation($"Start process at {DateTime.UtcNow}");

            try
            {
                var realmAuctions = await auctionHouseRepository.QueryMultipleAsync(new Auction()
                {
                    RealmName = realmName,
                    RealmFaction = realmFaction
                });

                var realmCurrentAuctions = await woWApiService.GetRealmAuctionsAsync(realmId, realmFaction);

                var currentRealmCurrentAuctionIds = realmCurrentAuctions.Auctions.Select(s => (long)s.Id).ToList();
                auctionsToDelete = realmAuctions.Where(i => !currentRealmCurrentAuctionIds.Contains(i.AuctionId)).ToList();

                foreach (var auction in realmCurrentAuctions.Auctions)
                {
                    if (realmAuctions.Any(a => a.AuctionId == auction.Id))
                    {
                        var auctionItemToUpdate = realmAuctions.FirstOrDefault(a => a.AuctionId == auction.Id);
                        MapUpdate(auction, auctionItemToUpdate);
                        auctionsToUpdate.Add(auctionItemToUpdate);
                    }
                    else
                    {
                        Auction auctionItemToInsert = MapNewItem(realmName, realmFaction, auction);
                        auctionsToInsert.Add(auctionItemToInsert);
                    }
                }

                await ProcessDatabaseTransactions(auctionsToInsert, auctionsToUpdate, auctionsToDelete);
            }
            catch (Exception e)
            {
                logger.LogError(e, $"Error on processing realmId={realmId} - realmName={realmName} - realmFaction={realmFaction}");
                throw;
            }

            sc.Stop();
            logger.LogInformation($"Ending process at {DateTime.UtcNow} in {sc.ElapsedMilliseconds / 1000}");

            return (true, auctionsToInsert.Count, auctionsToUpdate.Count, auctionsToDelete.Count);
        }

        private async Task ProcessDatabaseTransactions(List<Auction> itemsToInsert, List<Auction> itemsToUpdate, List<Auction> itemsToDelete)
        {
            logger.LogInformation($"Item to insert => {itemsToInsert.Count}");
            _ = await auctionHouseRepository.InsertsAsync(itemsToInsert.ToArray());

            logger.LogInformation($"Item to update => {itemsToUpdate.Count}");
            _ = await auctionHouseRepository.UpdatesAsync(itemsToUpdate.ToArray());

            logger.LogInformation($"Item to delete => {itemsToDelete.Count()}");
            _ = await auctionHouseRepository.DeletesAsync(itemsToDelete.ToArray());
        }

        private static Auction MapNewItem(string realmName, int realmFaction, Blizzard.WoWClassic.ApiClient.Contracts.Auction item)
        {
            return new Auction()
            {
                Id = Guid.NewGuid(),
                AuctionId = item.Id,
                ItemName = "ToDo",
                ItemId = item.Item.Id,
                ItemRand = item.Item.Rand,
                ItemSeed = item.Item.Seed,
                Quantity = item.Quantity,
                TimeLeft = item.TimeLeft,
                Bid = item.Bid,
                Buyout = item.Buyout,
                RealmFaction = realmFaction,
                RealmName = realmName,
                CreateAt = DateTime.UtcNow,
                CreateBy = "Worker"
            };
        }

        private static void MapUpdate(Blizzard.WoWClassic.ApiClient.Contracts.Auction item, Auction auctionItemToUpdate)
        {
            auctionItemToUpdate.UpdateAt = DateTime.UtcNow;
            auctionItemToUpdate.UpdateBy = "Worker";
            auctionItemToUpdate.Quantity = item.Quantity;
            auctionItemToUpdate.TimeLeft = item.TimeLeft;
            auctionItemToUpdate.Bid = item.Bid;
            auctionItemToUpdate.Buyout = item.Buyout;
        }
    }
}
