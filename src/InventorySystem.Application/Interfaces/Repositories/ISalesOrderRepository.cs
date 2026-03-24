using InventorySystem.Domain.Entities.SalesOrder;

namespace InventorySystem.Application.Interfaces.Repositories
{
    public interface ISalesOrderRepository : IRepository<SalesOrder>
    {
        Task<IEnumerable<SalesOrder>> GetAllWithLinesAsync(CancellationToken cancellationToken = default);
        Task<SalesOrder> GetWithLinesAsync(int id, CancellationToken cancellationToken = default);
        Task<List<SalesOrder>> GetConfirmedSalesOrders(CancellationToken cancellationToken = default);
    }
}
