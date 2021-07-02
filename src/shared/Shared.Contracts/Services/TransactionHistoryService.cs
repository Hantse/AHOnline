using Shared.Contracts.Interfaces;
using Shared.Contracts.Response;
using System;
using System.Net.Http;
using System.Text.Json;
using System.Threading.Tasks;

namespace Shared.Contracts.Services
{
    public class TransactionHistoryService : ITransactionHistoryService
    {
        public async Task<TransactionHistoryResponse[]> GetMonitoring()
        {
            using (var httpClient = new HttpClient()
            {
                BaseAddress = new Uri("http://194.163.135.185:8001")
            })
            {
                var httpResponse = await httpClient.GetAsync("TransactionHistory");
                if (httpResponse.IsSuccessStatusCode)
                {
                    var bodyAsString = await httpResponse.Content.ReadAsStringAsync();
                    return JsonSerializer.Deserialize<TransactionHistoryResponse[]>(bodyAsString);
                }
            }

            return default(TransactionHistoryResponse[]);
        }
    }
}
