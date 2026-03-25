using Inventory.Application.Interfaces.Repositories;
using Inventory.Domain.Entities.Inventory;
using Inventory.Infrastructure.Data;
using Microsoft.EntityFrameworkCore;

namespace Inventory.Infrastructure.Repositories
{
    public class InventoryReservationRepository : Repository<InventoryReservation>, IInventoryReservationRepository
    {
        public InventoryReservationRepository(ApplicationDbContext context) : base(context)
        {
        }

        public async Task<List<InventoryReservation>> GetReservationBySalesOrder(int salesOrderId, CancellationToken cancellationToken)
        {
            var reservations = await _context.InventoryReservations
                .Where(i => i.SourceId == salesOrderId && i.SourceType == "SalesOrder")
                .ToListAsync(cancellationToken);
            return reservations;
        }

        public async Task<InventoryReservation> GetReservation(int salesOrderId, int productId, int rowNumber, string sourceType, CancellationToken cancellationToken)
        {
            var reservation = await _context.InventoryReservations
                .Where(r => r.SourceId == salesOrderId && r.SourceType == sourceType && r.ProductId == productId && r.RowNumber == rowNumber)
                .FirstOrDefaultAsync(cancellationToken);
            return reservation;
        }
    }
}
