using InventorySystem.Application.DTOs.Inventory;
using InventorySystem.Application.Interfaces;
using InventorySystem.Domain.Entities.Inventory;
using InventorySystem.Infrastructure.Data;
using Microsoft.EntityFrameworkCore;

namespace InventorySystem.Infrastructure.Repositories;

public class InventoryCostLayerRepository : Repository<InventoryCostLayer>, IInventoryCostLayerRepository
{
    public InventoryCostLayerRepository(ApplicationDbContext context) : base(context)
    {
    }

    public async Task<List<InventoryCostLayerDto>> GetFIFOProductsById(int productId, CancellationToken cancellationToken = default)
    {
        var inventory_products = await _context.InventoryCostLayers
            .Where(i => i.ProductId == productId)
            .OrderBy(i => i.CreatedAt)
            .ToListAsync(cancellationToken);

        return inventory_products.Select(MapToDto).ToList();
    }

    private static InventoryCostLayerDto MapToDto(InventoryCostLayer entity)
    {
        return new InventoryCostLayerDto
        {
            GoodsReceiptId = entity.GoodsReceiptId,
            ProductId = entity.ProductId,
            WarehouseId = entity.WarehouseId,
            OriginalQty = entity.OriginalQty,
            RemainingQty = entity.RemainingQty,
            UnitCost = entity.UnitCost,
            ReceiptDate = entity.ReceiptDate,
            IsClosed = entity.IsClosed
        };
    }
}

