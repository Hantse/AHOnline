using Infrastructure.Core.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AHSync.Worker.Shared.Interfaces
{
    public interface IDiscordSyncRepository
    {
        Task<int> InsertSingleAsync(DiscordSync entity);
        Task<DiscordSync> QueryOneAsync(DiscordSync query);
    }
}
