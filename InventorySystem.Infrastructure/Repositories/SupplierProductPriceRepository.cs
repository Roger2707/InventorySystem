using InventorySystem.Application.Interfaces;
using InventorySystem.Domain.Entities.Suppliers;
using InventorySystem.Infrastructure.Data;

namespace InventorySystem.Infrastructure.Repositories;

public class SupplierProductPriceRepository : Repository<SupplierProductPrice>, ISupplierProductPriceRepository
{
    public SupplierProductPriceRepository(ApplicationDbContext context) : base(context)
    {
    }
}

