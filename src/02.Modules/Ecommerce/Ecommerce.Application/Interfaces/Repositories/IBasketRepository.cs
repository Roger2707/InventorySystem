using ECommerce.Domain.Entities.Baskets;

namespace ECommerce.Application.Interfaces.Repositories
{
    public interface IBasketRepository : IRepository<Basket>
    {
        Task<IEnumerable<Basket>> GetAllWithItemsAsync(CancellationToken cancellationToken = default);
        Task<Basket?> GetWithItemsAsync(int id, CancellationToken cancellationToken = default);
    }
}
