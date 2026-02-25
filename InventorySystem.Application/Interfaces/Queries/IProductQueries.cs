using InventorySystem.Application.DTOs.Products;
using InventorySystem.Domain.Common;

namespace InventorySystem.Application.Interfaces.Queries
{
    public interface IProductQueries
    {
        Task<IReadOnlyList<ProductDto>> GetAllAsync(CancellationToken cancellationToken);
        Task<ProductDto> GetByIdAsync(int id, CancellationToken cancellationToken);
    }
}
