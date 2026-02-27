using InventorySystem.Application.Interfaces;
using InventorySystem.Domain.Entities.GoodsReceipt;
using InventorySystem.Infrastructure.Data;
using Microsoft.EntityFrameworkCore;

namespace InventorySystem.Infrastructure.Repositories;

public class GoodsReceiptRepository : Repository<GoodsReceipt>, IGoodsReceiptRepository
{
    public GoodsReceiptRepository(ApplicationDbContext context) : base(context)
    {
    }

    public async Task<IEnumerable<GoodsReceipt>> GetAllWithLinesAsync(CancellationToken cancellationToken = default)
    {
        return await _dbSet
            .Include(x => x.Lines)
            .ToListAsync(cancellationToken);
    }

    public async Task<GoodsReceipt?> GetWithLinesAsync(int id, CancellationToken cancellationToken = default)
    {
        return await _dbSet
            .Include(x => x.Lines)
            .FirstOrDefaultAsync(x => x.Id == id, cancellationToken);
    }
}

