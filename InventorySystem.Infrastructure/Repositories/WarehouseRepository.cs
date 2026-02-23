using InventorySystem.Application.Interfaces;
using InventorySystem.Domain.Entities;
using InventorySystem.Infrastructure.Data;
using Microsoft.EntityFrameworkCore;

namespace InventorySystem.Infrastructure.Repositories;

public class WarehouseRepository : Repository<Warehouse>, IWarehouseRepository
{
    public WarehouseRepository(ApplicationDbContext context) 
        : base(context)
    {
    }

    public async Task<Warehouse?> GetByCodeAsync(string warehouseCode, CancellationToken cancellationToken = default)
    {
        return await _dbSet
            .FirstOrDefaultAsync(w => w.WarehouseCode == warehouseCode, cancellationToken);
    }

    public async Task<bool> ExistsByCodeAsync(string warehouseCode, CancellationToken cancellationToken = default)
    {
        return await _dbSet
            .AnyAsync(w => w.WarehouseCode == warehouseCode, cancellationToken);
    }

    public async Task<IEnumerable<Warehouse>> GetActiveWarehousesAsync(CancellationToken cancellationToken = default)
    {
        return await _dbSet
            .Where(w => w.IsActive == true)
            .OrderBy(w => w.WarehouseName)
            .ToListAsync(cancellationToken);
    }

    public async Task<IEnumerable<Warehouse>> GetWarehousesByManagerAsync(int managerId, CancellationToken cancellationToken = default)
    {
        var warehouses = await _dbSet
            .Where(w => w.UserWarehouses.Any(uw =>
                uw.UserId == managerId &&
                uw.IsWarehouseManager))
            .ToListAsync(cancellationToken);

        return warehouses;
    }

    public async Task<List<string>> GetAllWarehouseCodeAsync(CancellationToken cancellationToken)
    {
        var warehouseCodes = await _dbSet.Select(w => w.WarehouseCode).ToListAsync(cancellationToken);
        return warehouseCodes;
    }
}

