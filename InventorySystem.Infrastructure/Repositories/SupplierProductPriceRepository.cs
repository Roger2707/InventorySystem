using InventorySystem.Application.Interfaces;
using InventorySystem.Domain.Entities.Suppliers;
using InventorySystem.Infrastructure.Data;
using Microsoft.EntityFrameworkCore;

namespace InventorySystem.Infrastructure.Repositories;

public class SupplierProductPriceRepository : Repository<SupplierProductPrice>, ISupplierProductPriceRepository
{
    public SupplierProductPriceRepository(ApplicationDbContext context) : base(context)
    {
    }

    public async Task<decimal> GetTopUnitPrice(int supplierId, int productId, CancellationToken cancellationToken = default)
    {
        var price = await _dbSet
            .Where(spp => spp.SupplierId == supplierId && spp.ProductId == productId)
            .OrderByDescending(spp => spp.EffectiveDate)
            .Select(spp => spp.UnitPrice)
            .FirstOrDefaultAsync(cancellationToken);

        return price;
    }
}

