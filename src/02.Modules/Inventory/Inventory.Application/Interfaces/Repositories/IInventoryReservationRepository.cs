using Inventory.Domain.Entities.Inventory;

namespace Inventory.Application.Interfaces.Repositories
{
    public interface IInventoryReservationRepository : IRepository<InventoryReservation>
    {
        Task<List<InventoryReservation>> GetReservationBySalesOrder(int salesOrderId, CancellationToken cancellationToken);
        Task<InventoryReservation> GetReservation(int salesOrderId, int productId, int rowNumber, string sourceType, CancellationToken cancellationToken);
    }
}
