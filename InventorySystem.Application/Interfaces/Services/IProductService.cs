using InventorySystem.Application.DTOs.Products;
using InventorySystem.Domain.Common;

namespace InventorySystem.Application.Interfaces.Services
{
    public interface IProductService
    {
        Task<Result<List<ProductDto>>> GetAllAsync(CancellationToken cancellationToken);
        Task<Result<ProductDto>> GetByIdAsync(int id, CancellationToken cancellationToken);

        Task<Result<ProductDto>> CreateAsync(CreateProductDto createProductDto, CancellationToken cancellationToken);
        Task<Result<ProductDto>> UpdateAsync(int id, UpdateProductDto createProductDto, CancellationToken cancellationToken);
        Task<Result> DeleteAsync(int id, CancellationToken cancellationToken);
        Task<Result<bool>> ExistAsync(int id, CancellationToken cancellationToken);
    }
}