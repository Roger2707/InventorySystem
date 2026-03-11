using InventorySystem.Domain.Entities.Delivery;

namespace InventorySystem.Application.Interfaces
{
    public interface IDeliveryRepository : IRepository<Delivery>
    {
        Task<IEnumerable<Delivery>> GetAllWithLinesAsync(CancellationToken cancellationToken = default);
        Task<Delivery> GetWithLinesAsync(int id, CancellationToken cancellationToken = default);
    }
}
