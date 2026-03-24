using InventorySystem.Domain.Entities.Inventory;

namespace InventorySystem.Application.Interfaces.Repositories
{
    public interface IInventoryReservationRepository : IRepository<InventoryReservation>
    {
        Task<List<InventoryReservation>> GetReservationBySalesOrder(int salesOrderId, CancellationToken cancellationToken);
        Task<InventoryReservation> GetReservation(int salesOrderId, int productId, int rowNumber, string sourceType, CancellationToken cancellationToken);
    }
}
