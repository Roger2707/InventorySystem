using InventorySystem.Domain.Entities.Inventory;

namespace InventorySystem.Application.Interfaces
{
    public interface IInventoryReservationRepository : IRepository<InventoryReservation>
    {
        Task<List<InventoryReservation>> GetReservationBySalesOrder(int salesOrderId, CancellationToken cancellationToken);
    }
}
