using InventorySystem.Domain.Entities.Inventory;

namespace InventorySystem.Application.Interfaces;

public interface IInventoryCostLayerRepository : IRepository<InventoryCostLayer>
{
    Task<List<InventoryCostLayer>> GetFIFOProductsById(int productId, CancellationToken cancellationToken = default);
    Task<List<InventoryCostLayer>> GetByIdsAsync(IEnumerable<int> layerIds, CancellationToken cancellationToken = default);
}

