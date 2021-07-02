using Shared.Contracts.Response;
using System.Threading.Tasks;

namespace Service.Admin.Interfaces
{
    public interface ITransactionHistoryBusiness
    {
        Task<TransactionHistoryResponse[]> GetTransactionsHistoryAsync();
    }
}
