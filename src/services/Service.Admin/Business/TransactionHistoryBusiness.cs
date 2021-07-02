using AHSync.Worker.Shared.Interfaces;
using Service.Admin.Interfaces;
using Shared.Contracts.Response;
using System.Linq;
using System.Threading.Tasks;

namespace Service.Admin.Business
{
    public class TransactionHistoryBusiness : ITransactionHistoryBusiness
    {
        private readonly IOperationHistoryRepository operationHistoryRepository;

        public TransactionHistoryBusiness(IOperationHistoryRepository operationHistoryRepository)
        {
            this.operationHistoryRepository = operationHistoryRepository;
        }

        public async Task<TransactionHistoryResponse[]> GetTransactionsHistoryAsync()
        {
            var historyItems = await operationHistoryRepository.QueryMultipleAsync(new Infrastructure.Core.Entities.OperationHistory());
            var historyGrouping = historyItems.GroupBy(g => new { g.RealmFaction, g.RealmName }).ToList();

            return historyGrouping.Select(s =>
                                    new TransactionHistoryResponse()
                                    {
                                        RealmName = s.Key.RealmName,
                                        RealmFaction = s.Key.RealmFaction == 2 ? "Alliance" : "Horde",
                                        UpperTime = s.Max(s => s.Duration),
                                        AverageTime = s.Average(s => s.Duration),
                                        LowerTime = s.Min(s => s.Duration),
                                        MaxItem = s.Max(s => s.Inserted + s.Updated)
                                    })
                                    .OrderBy(o => o.RealmName)
                                    .ToArray();
        }
    }
}
