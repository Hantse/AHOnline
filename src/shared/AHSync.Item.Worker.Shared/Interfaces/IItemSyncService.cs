using System.Threading.Tasks;

namespace AHSync.Item.Worker.Shared.Interfaces
{
    public interface IItemSyncService
    {
        Task<(bool success, int insertResult)> TryProcessAsync(int startRange, int endRange);
    }
}
