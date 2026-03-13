using InventorySystem.Domain.Entities.Suppliers;

namespace InventorySystem.Application.Interfaces.Repositories;

public interface ISupplierProductPriceRepository : IRepository<SupplierProductPrice>
{
    Task<decimal> GetTopUnitPrice(int supplierId, int productId, CancellationToken cancellationToken = default);
}

