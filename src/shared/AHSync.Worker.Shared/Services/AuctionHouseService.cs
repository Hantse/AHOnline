using AHSync.Worker.Shared.Filters;
using AHSync.Worker.Shared.Interfaces;
using Blizzard.WoWClassic.ApiContract.Items;
using Infrastructure.Core.Entities;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text.Json;
using System.Threading.Tasks;

namespace AHSync.Worker.Shared.Services
{
    [Hangfire.Queue("ah-sync")]
    [RetryFilter]
    public class AuctionHouseService : IAuctionHouseService
    {
        private readonly IAuctionHouseRepository auctionHouseRepository;
        private readonly IItemRepository itemRepository;
        private readonly IOperationHistoryRepository operationHistoryRepository;
        private readonly IWoWApiService woWApiService;
        private readonly ILogger<AuctionHouseService> logger;

        private static IEnumerable<Item> items = new List<Item>();

        public AuctionHouseService(IAuctionHouseRepository auctionHouseRepository, ILogger<AuctionHouseService> logger, IWoWApiService woWApiService, IOperationHistoryRepository operationHistoryRepository, IItemRepository itemRepository)
        {
            this.auctionHouseRepository = auctionHouseRepository;
            this.logger = logger;
            this.woWApiService = woWApiService;
            this.operationHistoryRepository = operationHistoryRepository;
            this.itemRepository = itemRepository;
        }

        [Hangfire.Queue("ah-sync")]
        [RetryFilter]
        public async Task<(bool success, int insertResult, int updateResult, int deleteResult)> TryProcessAsync(int realmId, string realmName, int realmFaction)
        {
            var auctionsToInsert = new List<Auction>();
            var auctionsToUpdate = new List<Auction>();
            var auctionsToDelete = new List<Auction>();

            Stopwatch sc = new Stopwatch();
            sc.Start();

            logger.LogInformation($"Data receive : realmId={realmId} - realmName={realmName} - realmFaction={realmFaction}");
            logger.LogInformation($"Start process at {DateTime.UtcNow}");

            items = await itemRepository.QueryMultipleAsync(new Item());

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
                        if (auctionItemToUpdate != null)
                            auctionsToUpdate.Add(auctionItemToUpdate);
                    }
                    else
                    {
                        Auction auctionItemToInsert = MapNewItem(realmName, realmFaction, auction);
                        if (auctionItemToInsert != null)
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
            logger.LogInformation($"Ending process at {DateTime.UtcNow} in {sc.ElapsedMilliseconds} ms");

            _ = await operationHistoryRepository.InsertAsync(new OperationHistory()
            {
                CreateAt = DateTime.UtcNow,
                CreateBy = "System Worker",
                Duration = sc.ElapsedMilliseconds,
                Id = Guid.NewGuid(),
                RealmFaction = realmFaction,
                RealmName = realmName,
                Inserted = auctionsToInsert.Count,
                Updated = auctionsToUpdate.Count,
                Deleted = auctionsToDelete.Count
            });

            return (true, auctionsToInsert.Count, auctionsToUpdate.Count, auctionsToDelete.Count);
        }

        private async Task ProcessDatabaseTransactions(List<Auction> itemsToInsert, List<Auction> itemsToUpdate, List<Auction> itemsToDelete)
        {
            try
            {
                logger.LogInformation($"Item to insert => {itemsToInsert.Count}");
                _ = await auctionHouseRepository.InsertsAsync(itemsToInsert.ToArray());
            }
            catch (Exception e)
            {
                logger.LogError(e, "Error on insert ...");
            }

            try
            {
                logger.LogInformation($"Item to update => {itemsToUpdate.Count}");
                _ = await auctionHouseRepository.UpdatesAsync(itemsToUpdate.ToArray());
            }
            catch (Exception e)
            {
                logger.LogError(e, "Error on update ...");
            }

            try
            {
                logger.LogInformation($"Item to delete => {itemsToDelete.Count()}");
                _ = await auctionHouseRepository.DeletesAsync(itemsToDelete.ToArray());
            }
            catch (Exception e)
            {
                logger.LogError(e, "Error on delete ...");
            }
        }

        private static Auction MapNewItem(string realmName, int realmFaction, Blizzard.WoWClassic.ApiClient.Contracts.Auction item)
        {
            var dbItem = items.FirstOrDefault(f => f.ItemId == item.Item.Id);
            if (dbItem == null)
                return null;

            var itemDetails = JsonSerializer.Deserialize<ItemDetails>(dbItem.Value);

            return new Auction()
            {
                AuctionId = item.Id,
                ItemName = dbItem.NameEnUs,
                ItemNameFr = dbItem.NameFrFr,
                ItemClass = dbItem.ItemClass,
                ItemSubclass = dbItem.ItemSubClass,
                ItemId = item.Item.Id,
                ItemRand = item.Item.Rand,
                ItemSeed = item.Item.Seed,
                Quantity = item.Quantity,
                TimeLeft = item.TimeLeft,
                Quality = dbItem.Quality,
                InventoryType = itemDetails?.PreviewItem?.InventoryType?.Name?.EnUs,
                Level = itemDetails.RequiredLevel,
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
            var dbItem = items.FirstOrDefault(f => f.ItemId == item.Item.Id);
            if (dbItem == null)
            {
                auctionItemToUpdate = null;
            }
            else
            {
                var itemDetails = JsonSerializer.Deserialize<ItemDetails>(dbItem.Value);
                auctionItemToUpdate.UpdateAt = DateTime.UtcNow;
                auctionItemToUpdate.UpdateBy = "Worker";
                auctionItemToUpdate.Quantity = item.Quantity;
                auctionItemToUpdate.TimeLeft = item.TimeLeft;
                auctionItemToUpdate.Bid = item.Bid;
                auctionItemToUpdate.Buyout = item.Buyout;
            }
        }
    }
}
