using InventorySystem.Domain.Common;
using InventorySystem.Domain.Entities.Inventory;

namespace InventorySystem.Application.Interfaces.Services;

public interface IInventoryLedgerService
{
    Task<Result<IEnumerable<InventoryLedger>>> GetAllAsync(CancellationToken cancellationToken = default);
    Task<Result<InventoryLedger>> GetByIdAsync(int id, CancellationToken cancellationToken = default);
}

