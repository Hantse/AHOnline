using Microsoft.AspNetCore.Components;
using Shared.Contracts.Interfaces;
using Shared.Contracts.Response;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Admin.App.Pages
{
    public partial class Index : ComponentBase
    {
        [Inject]
        public ITransactionHistoryService TransactionHistoryService { get; set; }

        private List<TransactionHistoryResponse> transactions = new List<TransactionHistoryResponse>();

        protected override async Task OnInitializedAsync()
        {
            var transactionList = await TransactionHistoryService.GetMonitoring();
            transactions.AddRange(transactionList);
        }
    }
}
