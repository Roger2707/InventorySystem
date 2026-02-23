using InventorySystem.Domain.Entities;

namespace InventorySystem.Application.Interfaces;

public interface IWarehouseRepository : IRepository<Warehouse>
{
    Task<Warehouse?> GetByCodeAsync(string warehouseCode, CancellationToken cancellationToken = default);
    Task<bool> ExistsByCodeAsync(string warehouseCode, CancellationToken cancellationToken = default);
    Task<IEnumerable<Warehouse>> GetActiveWarehousesAsync(CancellationToken cancellationToken = default);
    Task<IEnumerable<Warehouse>> GetWarehousesByManagerAsync(int managerId, CancellationToken cancellationToken = default);
    Task<List<string>> GetAllWarehouseCodeAsync(CancellationToken cancellationToken);
}

