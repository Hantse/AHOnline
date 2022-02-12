using Infrastructure.Core.Entities;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace AHSync.Worker.Shared.Interfaces
{
    public interface IItemRepository
    {
        Task<IEnumerable<Item>> QueryMultipleAsync(Item query);
    }
}
