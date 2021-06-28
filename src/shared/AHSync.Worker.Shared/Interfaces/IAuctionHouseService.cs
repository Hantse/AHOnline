using System.Threading.Tasks;

namespace AHSync.Worker.Shared.Interfaces
{
    public interface IAuctionHouseService
    {
        Task<(bool success, int insertResult, int updateResult, int deleteResult)> TryProcessAsync(int realmId, string realmName, int realmFaction);
    }
}
