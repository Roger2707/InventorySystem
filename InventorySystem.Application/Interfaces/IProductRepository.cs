using InventorySystem.Domain.Entities;

namespace InventorySystem.Application.Interfaces
{
    public interface IProductRepository : IRepository<Product>
    {
        Task<Product> GetWithConversionAsync(int id, CancellationToken cancellationToken = default);
    }
}
