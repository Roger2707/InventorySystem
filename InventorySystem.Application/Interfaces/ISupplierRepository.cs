using InventorySystem.Domain.Entities.Suppliers;

namespace InventorySystem.Application.Interfaces;

public interface ISupplierRepository : IRepository<Supplier>
{
    Task<Supplier?> GetByCodeAsync(string supplierCode, CancellationToken cancellationToken = default);
    Task<bool> ExistsByCodeAsync(string supplierCode, CancellationToken cancellationToken = default);
    Task<List<string>> GetAllSupplierCodeAsync(CancellationToken cancellationToken);
}

