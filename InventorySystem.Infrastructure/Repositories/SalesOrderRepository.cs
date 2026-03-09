using InventorySystem.Application.Interfaces;
using InventorySystem.Domain.Entities.SalesOrder;
using InventorySystem.Infrastructure.Data;
using Microsoft.EntityFrameworkCore;

namespace InventorySystem.Infrastructure.Repositories
{
    public class SalesOrderRepository : Repository<SalesOrder>, ISalesOrderRepository
    {
        public SalesOrderRepository(ApplicationDbContext context) : base(context)
        {
        }

        public async Task<IEnumerable<SalesOrder>> GetAllWithLinesAsync(CancellationToken cancellationToken = default)
        {
            var salesOrder = await _context.SalesOrders
                .Include(s => s.Customer)
                .Include(s => s.Lines)
                .ThenInclude(l => l.Product)
                .ToListAsync(cancellationToken);

            return salesOrder;
        }

        public async Task<SalesOrder> GetWithLinesAsync(int id, CancellationToken cancellationToken = default)
        {
            var salesOrder = await _context.SalesOrders
                                        .Include(s => s.Customer)
                                        .Include(s => s.Lines)
                                        .ThenInclude(l => l.Product)
                                        .FirstOrDefaultAsync(x => x.Id == id, cancellationToken);
            return salesOrder;
        }
    }
}
