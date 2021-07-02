using Microsoft.AspNetCore.Mvc;
using Service.Admin.Interfaces;
using System.Threading.Tasks;

namespace Service.Admin.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class TransactionHistoryController : ControllerBase
    {
        private readonly ITransactionHistoryBusiness transactionHistoryBusiness;

        public TransactionHistoryController(ITransactionHistoryBusiness transactionHistoryBusiness)
        {
            this.transactionHistoryBusiness = transactionHistoryBusiness;
        }

        [HttpGet]
        public async Task<IActionResult> GetStatsAsync()
        {
            return Ok(await transactionHistoryBusiness.GetTransactionsHistoryAsync());
        }
    }
}
