using Blizzard.WoWClassic.ApiClient.Contracts;
using System.Threading.Tasks;

namespace AHSync.Worker.Shared.Interfaces
{
    public interface IWoWApiService
    {
        Task<AuctionHouseAuction> GetRealmAuctionsAsync(int realmId, int realmFaction);
    }
}
