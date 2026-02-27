using InventorySystem.Application.Interfaces;
using InventorySystem.Domain.Entities.PurchaseOrder;
using InventorySystem.Infrastructure.Data;
using Microsoft.EntityFrameworkCore;

namespace InventorySystem.Infrastructure.Repositories;

public class PurchaseOrderRepository : Repository<PurchaseOrder>, IPurchaseOrderRepository
{
    public PurchaseOrderRepository(ApplicationDbContext context) : base(context)
    {
    }

    public async Task<IEnumerable<PurchaseOrder>> GetAllWithLinesAsync(CancellationToken cancellationToken = default)
    {
        return await _dbSet
            .Include(x => x.Lines)
            .ToListAsync(cancellationToken);
    }

    public async Task<PurchaseOrder?> GetWithLinesAsync(int id, CancellationToken cancellationToken = default)
    {
        return await _dbSet
            .Include(x => x.Lines)
            .FirstOrDefaultAsync(x => x.Id == id, cancellationToken);
    }
}

