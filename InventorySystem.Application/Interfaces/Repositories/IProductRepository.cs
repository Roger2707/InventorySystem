using InventorySystem.Domain.Entities.Products;

namespace InventorySystem.Application.Interfaces.Repositories
{
    public interface IProductRepository : IRepository<Product>
    {
        Task<Product> GetWithConversionAsync(int id, CancellationToken cancellationToken = default);
    }
}
