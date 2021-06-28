using Infrastructure.Core.Entities;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace AHSync.Worker.Shared.Interfaces
{
    public interface IOperationHistoryRepository
    {
        Task<Guid?> InsertAsync(OperationHistory entity);
        Task<IEnumerable<OperationHistory>> QueryMultipleAsync(OperationHistory query);
    }
}
