using InventorySystem.Application.Interfaces.Repositories;
using InventorySystem.Domain.Entities.Baskets;
using InventorySystem.Infrastructure.Data;
using Microsoft.EntityFrameworkCore;

namespace InventorySystem.Infrastructure.Repositories
{
    public class BasketRepository : Repository<Basket>, IBasketRepository
    {
        public BasketRepository(ApplicationDbContext context) : base(context)
        {
        }

        public async Task<IEnumerable<Basket>> GetAllWithItemsAsync(CancellationToken cancellationToken = default)
        {
            return await _context.Baskets
                .Include(x => x.Customer)
                .Include(x => x.Items)
                .ThenInclude(x => x.Product)
                .ToListAsync(cancellationToken);
        }

        public async Task<Basket?> GetWithItemsAsync(int id, CancellationToken cancellationToken = default)
        {
            return await _context.Baskets
                .Include(x => x.Customer)
                .Include(x => x.Items)
                .ThenInclude(x => x.Product)
                .FirstOrDefaultAsync(x => x.Id == id, cancellationToken);
        }
    }
}
