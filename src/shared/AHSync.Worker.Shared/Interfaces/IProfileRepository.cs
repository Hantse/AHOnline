using Infrastructure.Core.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AHSync.Worker.Shared.Interfaces
{
    public interface IProfileRepository
    {
        Task<int> InsertSingleAsync(Profile entity);
        Task<Profile> QueryOneAsync(Profile query);
    }
}
