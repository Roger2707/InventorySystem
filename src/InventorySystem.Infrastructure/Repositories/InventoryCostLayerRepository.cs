using InventorySystem.Application.Interfaces.Repositories;
using InventorySystem.Domain.Entities.Inventory;
using InventorySystem.Infrastructure.Data;
using Microsoft.EntityFrameworkCore;

namespace InventorySystem.Infrastructure.Repositories;

public class InventoryCostLayerRepository : Repository<InventoryCostLayer>, IInventoryCostLayerRepository
{
    public InventoryCostLayerRepository(ApplicationDbContext context) : base(context)
    {
    }

    public async Task<List<InventoryCostLayer>> GetFIFOProductsById(int productId, CancellationToken cancellationToken = default)
    {
        var layers = await _context.InventoryCostLayers
            .Where(i => i.ProductId == productId && i.RemainingQty - i.ReservedQty > 0)
            .OrderBy(i => i.ReceiptDate)
            .ToListAsync(cancellationToken);

        return layers;
    }

    public async Task<List<InventoryCostLayer>> GetByIdsAsync(IEnumerable<int> layerIds, CancellationToken cancellationToken = default)
    {
        return await _context.InventoryCostLayers
            .Where(x => layerIds.Contains(x.Id))
            .ToListAsync(cancellationToken);
    }
}

