using AHSync.Item.Worker.Shared.Interfaces;
using Blizzard.WoWClassic.ApiClient;
using Blizzard.WoWClassic.ApiClient.Helpers;
using Microsoft.Extensions.Logging;
using System;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Text.Json;
using System.Threading.Tasks;

namespace AHSync.Item.Worker.Shared.Services
{
    public class ItemSyncService : IItemSyncService
    {
        private readonly ILogger<ItemSyncService> logger;
        private const string BasePath = "Items";
        private const string BasePathMadia = "Medias";

        public ItemSyncService(ILogger<ItemSyncService> logger)
        {
            this.logger = logger;
            if (!Directory.Exists(BasePath))
            {
                Directory.CreateDirectory(BasePath);
                Directory.CreateDirectory(BasePathMadia);
            }
            else
            {
                logger.LogInformation("Path already exist.");
            }
         
            logger.LogInformation($"Working on {Directory.GetCurrentDirectory()}");
        }

        public async Task<(bool success, int insertResult)> TryProcessAsync(int startRange, int endRange)
        {
            var itemToInsert = 0;

            Stopwatch sc = new Stopwatch();
            sc.Start();

            logger.LogInformation($"Data receive : item from {startRange} to {endRange}");
            logger.LogInformation($"Start process at {DateTime.UtcNow}");

            var clientWow = new WoWClassicApiClient("bxSvhNNHJwI0kgNvKy6Z91oMEOpwgjmv", "2b136112d3064b11b19c5ea275846996");
            clientWow.SetDefaultValues(RegionHelper.Europe, NamespaceHelper.Static, LocaleHelper.French);

            for (var i = startRange; i < endRange; i++)
            {
                try
                {
                    var itemDetails = await clientWow.GetItemDetailsAsync(i);
                    if (itemDetails == null)
                    {
                        logger.LogWarning($"Error not found itemId={i}");
                    }
                    else
                    {
                        if (itemDetails.PreviewItem.Binding == null || itemDetails.PreviewItem.Binding.Type != "ON_ACQUIRE")
                        {
                            await File.WriteAllTextAsync($"{BasePath}/{itemDetails.Id}.json", JsonSerializer.Serialize(itemDetails));

                            var itemMedia = await clientWow.GetItemMediaAsync(itemDetails.Id, RegionHelper.Us, NamespaceHelper.Static);

                            if (itemMedia != null && itemMedia.Assets.Any())
                            {
                                var downloadResult = await clientWow.DownloadMediaAsync(itemMedia.Assets[0].Value, $"{itemDetails.Id}.png");
                                File.WriteAllBytes($"Medias/item_{itemDetails.Id}.png", downloadResult.Data);
                            }
                        }
                    }
                }
                catch (Exception e)
                {
                    logger.LogError(e, $"Error on processing itemId={i}");
                }
            }

            sc.Stop();
            logger.LogInformation($"Ending process at {DateTime.UtcNow} in {sc.ElapsedMilliseconds} ms");

            return (true, itemToInsert);
        }
    }
}
