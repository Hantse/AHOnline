using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace AHSync.Worker.Shared.Interfaces
{
    public interface IAuctionHouseService
    {
        Task<bool> TryProcessAsync(int realmId, string realmName, int realmFaction);
    }
}
