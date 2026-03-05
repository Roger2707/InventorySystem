using InventorySystem.Application.DTOs.Inventory;
using InventorySystem.Domain.Common;

namespace InventorySystem.Application.Interfaces.Services;

public interface IInventoryCostLayerService
{
    Task<Result<IEnumerable<InventoryCostLayerDto>>> GetAllAsync(CancellationToken cancellationToken = default);
    Task<Result<InventoryCostLayerDto>> GetByIdAsync(int id, CancellationToken cancellationToken = default);
}

