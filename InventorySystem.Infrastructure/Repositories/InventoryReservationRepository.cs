using InventorySystem.Application.Interfaces;
using InventorySystem.Domain.Entities.Inventory;
using InventorySystem.Infrastructure.Data;
using Microsoft.EntityFrameworkCore;

namespace InventorySystem.Infrastructure.Repositories
{
    public class InventoryReservationRepository : Repository<InventoryReservation>, IInventoryReservationRepository
    {
        public InventoryReservationRepository(ApplicationDbContext context) : base(context)
        {
        }

        public async Task<List<InventoryReservation>> GetReservationBySalesOrder(int salesOrderId, CancellationToken cancellationToken)
        {
            var reservation = await _context.InventoryReservations
                .Where(i => i.SourceId == salesOrderId && i.SourceType == "SalesOrder")
                .ToListAsync(cancellationToken);
            return reservation;
        }
    }
}
