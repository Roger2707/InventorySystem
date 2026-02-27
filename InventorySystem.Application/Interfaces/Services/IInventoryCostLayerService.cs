using InventorySystem.Domain.Common;
using InventorySystem.Domain.Entities.Inventory;

namespace InventorySystem.Application.Interfaces.Services;

public interface IInventoryCostLayerService
{
    Task<Result<IEnumerable<InventoryCostLayer>>> GetAllAsync(CancellationToken cancellationToken = default);
    Task<Result<InventoryCostLayer>> GetByIdAsync(int id, CancellationToken cancellationToken = default);
}

