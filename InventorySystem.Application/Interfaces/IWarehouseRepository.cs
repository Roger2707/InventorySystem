using InventorySystem.Domain.Entities;

namespace InventorySystem.Application.Interfaces;

public interface IWarehouseRepository : IRepository<Warehouse>
{
    /// <summary>
    /// Gets a warehouse by its unique code
    /// </summary>
    Task<Warehouse?> GetByCodeAsync(string warehouseCode, CancellationToken cancellationToken = default);

    /// <summary>
    /// Checks if a warehouse with the given code exists
    /// </summary>
    Task<bool> ExistsByCodeAsync(string warehouseCode, CancellationToken cancellationToken = default);

    /// <summary>
    /// Gets all active warehouses
    /// </summary>
    Task<IEnumerable<Warehouse>> GetActiveWarehousesAsync(CancellationToken cancellationToken = default);

    /// <summary>
    /// Gets warehouses by manager ID
    /// </summary>
    Task<IEnumerable<Warehouse>> GetWarehousesByManagerAsync(int managerId, CancellationToken cancellationToken = default);
}

