using InventorySystem.Application.Interfaces;
using InventorySystem.Domain.Delivery;
using InventorySystem.Infrastructure.Data;
using Microsoft.EntityFrameworkCore;

namespace InventorySystem.Infrastructure.Repositories
{
    public class DeliveryRepository : Repository<Delivery>, IDeliveryRepository
    {
        public DeliveryRepository(ApplicationDbContext context) : base(context)
        {
        }

        public async Task<IEnumerable<Delivery>> GetAllWithLinesAsync(CancellationToken cancellationToken = default)
        {
            var deliveries = await _context.Deliveries.Include(d => d.Lines).ToListAsync();
            return deliveries;
        }

        public async Task<Delivery> GetWithLinesAsync(int id, CancellationToken cancellationToken = default)
        {
            var delivery = await _context.Deliveries
                .Include(x => x.Lines)
                .FirstOrDefaultAsync(d => d.Id == id, cancellationToken);
            return delivery;
        }
    }
}
