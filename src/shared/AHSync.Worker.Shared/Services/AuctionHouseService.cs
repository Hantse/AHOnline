using AHSync.Worker.Shared.Interfaces;
using Blizzard.WoWClassic.ApiClient;
using Blizzard.WoWClassic.ApiClient.Helpers;
using Infrastructure.Core.Entities;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;

namespace AHSync.Worker.Shared.Services
{
    public class AuctionHouseService : IAuctionHouseService
    {
        private readonly IAuctionHouseRepository auctionHouseRepository;
        private readonly ILogger<AuctionHouseService> logger;

        public AuctionHouseService(IAuctionHouseRepository auctionHouseRepository, ILogger<AuctionHouseService> logger)
        {
            this.auctionHouseRepository = auctionHouseRepository;
            this.logger = logger;
        }

        public async Task<bool> TryProcessAsync(int realmId, string realmName, int realmFaction)
        {
            Stopwatch sc = new Stopwatch();
            sc.Start();

            logger.LogInformation($"Data receive : realmId={realmId} - realmName={realmName} - realmFaction={realmFaction}");
            logger.LogInformation($"Start process at {DateTime.UtcNow}");

            try
            {
                var clientWow = new WoWClassicApiClient("bxSvhNNHJwI0kgNvKy6Z91oMEOpwgjmv", "2b136112d3064b11b19c5ea275846996");
                clientWow.SetDefaultValues(RegionHelper.Europe, NamespaceHelper.Dynamic, LocaleHelper.French);

                var itemsToInsert = new List<Auction>();
                var itemsToUpdate = new List<Auction>();
                var itemsToDelete = new List<Auction>();

                var realmAuctions = await auctionHouseRepository.QueryMultipleAsync(new Auction()
                {
                    RealmName = realmName,
                    RealmFaction = realmFaction
                });

                var realmCurrentAuctions = await clientWow.GetRealmAuctionsAsync(realmId, realmFaction, RegionHelper.Europe, NamespaceHelper.Dynamic, LocaleHelper.French);

                var currentRealmCurrentAuctionIds = realmCurrentAuctions.Auctions.Select(s => (long)s.Id).ToList();
                var itemsToDeleteIds = realmAuctions.Where(s => !currentRealmCurrentAuctionIds.Contains(s.AuctionId)).Select(s => s.AuctionId).ToArray();

                foreach (var item in realmCurrentAuctions.Auctions)
                {
                    if (itemsToDeleteIds.Contains(item.Id))
                    {
                        var auctionItemToUpdate = realmAuctions.FirstOrDefault(a => a.AuctionId == item.Id);
                        MapUpdate(item, auctionItemToUpdate);
                        itemsToUpdate.Add(auctionItemToUpdate);
                    }
                    else if (realmAuctions.Any(a => a.AuctionId == item.Id))
                    {
                        var auctionItemToDelete = realmAuctions.FirstOrDefault(a => a.AuctionId == item.Id);
                        MapDelete(auctionItemToDelete);
                        itemsToDelete.Add(auctionItemToDelete);
                    }
                    else
                    {
                        Auction auctionItemToInsert = MapNewItem(realmName, realmFaction, item);
                        itemsToInsert.Add(auctionItemToInsert);
                    }
                }

                await ProcessDatabaseTransactions(itemsToInsert, itemsToUpdate, itemsToDelete);
            }
            catch (Exception e)
            {
                logger.LogError(e, $"Error on processing realmId={realmId} - realmName={realmName} - realmFaction={realmFaction}");
            }

            sc.Stop();
            logger.LogInformation($"Ending process at {DateTime.UtcNow} in {sc.ElapsedMilliseconds / 1000}");

            return true;
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

        private static void MapDelete(Auction auctionItemToDelete)
        {
            auctionItemToDelete.DeleteAt = DateTime.UtcNow;
            auctionItemToDelete.DeleteBy = "Worker";
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
