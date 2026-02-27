using InventorySystem.Domain.Entities.PurchaseOrder;

namespace InventorySystem.Application.Interfaces;

public interface IPurchaseOrderRepository : IRepository<PurchaseOrder>
{
    Task<IEnumerable<PurchaseOrder>> GetAllWithLinesAsync(CancellationToken cancellationToken = default);
    Task<PurchaseOrder?> GetWithLinesAsync(int id, CancellationToken cancellationToken = default);
}

