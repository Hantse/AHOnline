using AHSync.Worker.Shared.Queries;
using Infrastructure.Core.Entities;
using Service.Api.Interfaces;
using System.Threading.Tasks;

namespace Service.Api.Business
{
    public class AuctionBusiness : IAuctionBusiness
    {
        public async Task<Auction> QueryAuctionAsync(AuctionHouseQuery query)
        {

        }
    }
}
