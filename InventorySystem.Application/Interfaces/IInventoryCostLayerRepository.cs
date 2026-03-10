using InventorySystem.Application.DTOs.Inventory;
using InventorySystem.Domain.Entities.Inventory;

namespace InventorySystem.Application.Interfaces;

public interface IInventoryCostLayerRepository : IRepository<InventoryCostLayer>
{
    Task<List<InventoryCostLayerDto>> GetFIFOProductsById(int productId, CancellationToken cancellationToken = default);
}

