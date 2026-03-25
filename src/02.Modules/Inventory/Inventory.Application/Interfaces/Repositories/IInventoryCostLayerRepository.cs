using Inventory.Domain.Entities.Inventory;

namespace Inventory.Application.Interfaces.Repositories;

public interface IInventoryCostLayerRepository : IRepository<InventoryCostLayer>
{
    Task<List<InventoryCostLayer>> GetFIFOProductsById(int productId, CancellationToken cancellationToken = default);
    Task<List<InventoryCostLayer>> GetByIdsAsync(IEnumerable<int> layerIds, CancellationToken cancellationToken = default);
}

