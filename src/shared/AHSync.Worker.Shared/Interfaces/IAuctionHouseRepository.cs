using AHSync.Worker.Shared.Queries;
using Infrastructure.Core.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace AHSync.Worker.Shared.Interfaces
{
    public interface IAuctionHouseRepository
    {
        Task<int> DeleteAsync(Auction entity);
        Task<int> DeletesAsync(Auction[] entities);
        Task<Guid?> InsertAsync(Auction entity);
        Task<int> InsertsAsync(Auction[] entities);
        Task<IEnumerable<Auction>> QueryMultipleAsync(Auction query);
        Task<IEnumerable<Auction>> QueryFilterAsync(AuctionHouseQuery query);
        Task<IEnumerable<Auction>> QueryMultipleByIdAsync(Guid[] ids);
        Task<Auction> QueryOneAsync(Auction query);
        Task<Auction> QueryOneByIdAsync(Guid id);
        Task<int> UpdateAsync(Auction entity);
        Task<int> UpdatesAsync(Auction[] entities);
    }
}
