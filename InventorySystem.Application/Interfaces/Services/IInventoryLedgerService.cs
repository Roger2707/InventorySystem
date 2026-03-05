using InventorySystem.Application.DTOs.Inventory;
using InventorySystem.Domain.Common;

namespace InventorySystem.Application.Interfaces.Services;

public interface IInventoryLedgerService
{
    Task<Result<IEnumerable<InventoryLedgerDto>>> GetAllAsync(CancellationToken cancellationToken = default);
    Task<Result<InventoryLedgerDto>> GetByIdAsync(int id, CancellationToken cancellationToken = default);
}

