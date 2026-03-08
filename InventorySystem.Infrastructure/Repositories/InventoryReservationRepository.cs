using InventorySystem.Application.Interfaces;
using InventorySystem.Domain.Entities.Inventory;
using InventorySystem.Infrastructure.Data;

namespace InventorySystem.Infrastructure.Repositories
{
    public class InventoryReservationRepository : Repository<InventoryReservation>, IInventoryReservationRepository
    {
        public InventoryReservationRepository(ApplicationDbContext context) : base(context)
        {
        }
    }
}
