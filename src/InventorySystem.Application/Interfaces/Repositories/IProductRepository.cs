using InventorySystem.Application.Common.Pagination;
using InventorySystem.Application.DTOs.Products;
using InventorySystem.Domain.Entities.Products;

namespace InventorySystem.Application.Interfaces.Repositories
{
    public interface IProductRepository : IRepository<Product>
    {
        Task<PagedResult<ProductDto>> GetProductsPagedAsync(ProductParams productParams);
        Task<Product> GetWithConversionAsync(int id, CancellationToken cancellationToken = default);
    }
}
