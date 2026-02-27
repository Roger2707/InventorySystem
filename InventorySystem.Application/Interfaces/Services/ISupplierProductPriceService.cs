using InventorySystem.Domain.Common;
using InventorySystem.Domain.Entities.Suppliers;

namespace InventorySystem.Application.Interfaces.Services;

public interface ISupplierProductPriceService
{
    Task<Result<IEnumerable<SupplierProductPrice>>> GetAllAsync(CancellationToken cancellationToken = default);
    Task<Result<SupplierProductPrice>> GetByIdAsync(int id, CancellationToken cancellationToken = default);
}

