using Inventory.Application.Common.Pagination;
using Inventory.Application.DTOs.Products;
using Inventory.Domain.Entities.Products;
using SharedKernel;

namespace Inventory.Application.Interfaces.Repositories
{
    public interface IProductRepository : IRepository<Product>
    {
        Task<PagedResult<ProductDto>> GetProductsPagedAsync(ProductParams productParams);
        Task<Product> GetWithConversionAsync(int id, CancellationToken cancellationToken = default);
    }
}

