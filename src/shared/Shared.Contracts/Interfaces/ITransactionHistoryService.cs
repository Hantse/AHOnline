using Shared.Contracts.Response;
using System.Threading.Tasks;

namespace Shared.Contracts.Interfaces
{
    public interface ITransactionHistoryService
    {
        Task<TransactionHistoryResponse[]> GetMonitoring();
    }
}
