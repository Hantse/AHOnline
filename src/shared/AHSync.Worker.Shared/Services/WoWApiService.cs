using AHSync.Worker.Shared.Interfaces;
using Blizzard.WoWClassic.ApiClient;
using Blizzard.WoWClassic.ApiClient.Contracts;
using Blizzard.WoWClassic.ApiClient.Helpers;
using System.Threading.Tasks;

namespace AHSync.Worker.Shared.Services
{
    public class WoWApiService : IWoWApiService
    {
        private readonly WoWClassicApiClient clientWow;

        public WoWApiService()
        {
            clientWow = new WoWClassicApiClient("bxSvhNNHJwI0kgNvKy6Z91oMEOpwgjmv", "2b136112d3064b11b19c5ea275846996");
            clientWow.SetDefaultValues(RegionHelper.Europe, NamespaceHelper.Dynamic, LocaleHelper.French);
        }

        public Task<AuctionHouseAuction> GetRealmAuctionsAsync(int realmId, int realmFaction)
        {
            return clientWow.GetRealmAuctionsAsync(realmId, realmFaction, RegionHelper.Europe, NamespaceHelper.Dynamic, LocaleHelper.French);
        }
    }
}
