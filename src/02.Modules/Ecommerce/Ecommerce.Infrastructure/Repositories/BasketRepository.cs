using ECommerce.Application.Interfaces.Repositories;
using ECommerce.Domain.Entities.Baskets;
using ECommerce.Infrastructure.Data;
using Microsoft.EntityFrameworkCore;

namespace ECommerce.Infrastructure.Repositories
{
    public class BasketRepository : Repository<Basket>, IBasketRepository
    {
        public BasketRepository(ECommerceDbContext context) : base(context)
        {
        }

        public async Task<IEnumerable<Basket>> GetAllWithItemsAsync(CancellationToken cancellationToken = default)
        {
            return await _context.Baskets
                .Include(x => x.Items)
                .ToListAsync(cancellationToken);
        }

        public async Task<Basket?> GetWithItemsAsync(int id, CancellationToken cancellationToken = default)
        {
            return await _context.Baskets
                .Include(x => x.Items)
                .FirstOrDefaultAsync(x => x.Id == id, cancellationToken);
        }
    }
}
