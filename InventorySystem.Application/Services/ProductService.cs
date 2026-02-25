using InventorySystem.Application.DTOs.Products;
using InventorySystem.Application.Interfaces;
using InventorySystem.Application.Interfaces.Queries;
using InventorySystem.Application.Interfaces.Services;
using InventorySystem.Domain.Common;

namespace InventorySystem.Application.Services
{
    public class ProductService : IProductService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IProductQueries _productQueries;

        public ProductService(IUnitOfWork unitOfWork, IProductQueries productQueries)
        {
            _unitOfWork = unitOfWork;
            _productQueries = productQueries;
        }

        public Task<Result<List<ProductDto>>> GetAllAsync(CancellationToken cancellationToken)
        {
            throw new NotImplementedException();
        }

        public Task<Result<ProductDto>> GetByIdAsync(int id, CancellationToken cancellationToken)
        {
            throw new NotImplementedException();
        }

        public Task<Result<ProductDto>> CreateAsync(CreateProductDto createProductDto, CancellationToken cancellationToken)
        {
            throw new NotImplementedException();
        }

        public Task<Result> DeleteAsync(int id, CancellationToken cancellationToken)
        {
            throw new NotImplementedException();
        }

        public Task<Result<bool>> ExistAsync(int id, CancellationToken cancellationToken)
        {
            throw new NotImplementedException();
        }

        public Task<Result<ProductDto>> UpdateAsync(int id, UpdateProductDto createProductDto, CancellationToken cancellationToken)
        {
            throw new NotImplementedException();
        }
    }
}
