using InventorySystem.Application.DTOs.Products;
using InventorySystem.Domain.Common;

namespace InventorySystem.Application.Interfaces.Queries
{
    public interface IProductQueries
    {
        Task<Result<List<ProductDto>>> GetAllAsync(CancellationToken cancellationToken);
        Task<Result<ProductDto>> GetByIdAsync(int id, CancellationToken cancellationToken);
    }
}
