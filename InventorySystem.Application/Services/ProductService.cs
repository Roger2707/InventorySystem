using InventorySystem.Application.DTOs.Products;
using InventorySystem.Application.Interfaces;
using InventorySystem.Application.Interfaces.Queries;
using InventorySystem.Application.Interfaces.Services;
using InventorySystem.Domain.Common;
using InventorySystem.Domain.Entities;

namespace InventorySystem.Application.Services
{
    public class ProductService : IProductService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IProductQueries _productQueries;
        private readonly ISkuGenerator _skuGenerator;
        private readonly IBarcodeGenerator _barcodeGenerator;

        public ProductService(IUnitOfWork unitOfWork, IProductQueries productQueries, ISkuGenerator skuGenerator, IBarcodeGenerator barcodeGenerator)
        {
            _unitOfWork = unitOfWork;
            _productQueries = productQueries;
            _skuGenerator = skuGenerator;
            _barcodeGenerator = barcodeGenerator;
        }

        public async Task<Result<List<ProductDto>>> GetAllAsync(CancellationToken cancellationToken)
        {
            var products = await _productQueries.GetAllAsync(cancellationToken);
            return Result<List<ProductDto>>.Success(products.ToList());
        }

        public async Task<Result<ProductDto>> GetByIdAsync(int id, CancellationToken cancellationToken)
        {
            var product = await _productQueries.GetByIdAsync(id, cancellationToken);
            if (product == null)
            {
                return Result<ProductDto>.Failure($"Product with ID {id} not found.");
            }
            return Result<ProductDto>.Success(product);
        }

        public async Task<Result<ProductDto>> CreateAsync(CreateProductDto createProductDto, CancellationToken cancellationToken)
        {
            if(!IsDataValid(createProductDto, out var validationMessage))
            {
                return Result<ProductDto>.Failure(validationMessage);
            }
            var sku = await _skuGenerator.GenerateAsync(cancellationToken);
            var barcode = await _barcodeGenerator.GenerateAsync(cancellationToken);

            var newProduct = new Product
            {
                Name = createProductDto.Name,
                SKU = sku,
                Barcode = barcode,
                CategoryId = createProductDto.CategoryId,
                BaseUoMId = createProductDto.BaseUoMId,
                MinStockLevel = createProductDto.MinStockLevel,
            };

            if(createProductDto.ConversionDtos.Any())
            {
                var conversions = createProductDto.ConversionDtos
                .Select(c => new ProductUoMConversion
                {
                    ProductId = newProduct.Id,
                    FromUoMId = c.FromUoMId,
                    ToUoMId = c.ToUoMId,
                    Factor = c.Factor
                })
                .ToList();
                newProduct.Conversions = conversions;
            }

            await _unitOfWork.ProductRepository.AddAsync(newProduct, cancellationToken);
            await _unitOfWork.SaveChangesAsync(cancellationToken);

            var productDto = MapToDto(newProduct);
            return Result<ProductDto>.Success(productDto);
        }

        public async Task<Result<ProductDto>> UpdateAsync(int id, UpdateProductDto updateProductDto, CancellationToken cancellationToken)
        {
            if(!IsDataValid(updateProductDto, out var validationMessage))
            {
                return Result<ProductDto>.Failure(validationMessage);
            }
            var existedProduct = await _unitOfWork.ProductRepository.GetWithConversionAsync(id, cancellationToken);

            if(existedProduct == null)
            {
                return Result<ProductDto>.Failure($"Product with ID {id} not found.");
            }
            existedProduct.Name = updateProductDto.Name;
            existedProduct.CategoryId = updateProductDto.CategoryId;
            existedProduct.BaseUoMId = updateProductDto.BaseUoMId;
            existedProduct.MinStockLevel = updateProductDto.MinStockLevel;

            existedProduct.Conversions.Clear();
            foreach (var dto in updateProductDto.ConversionDtos)
            {
                existedProduct.Conversions.Add(new ProductUoMConversion
                {
                    ProductId = existedProduct.Id,
                    FromUoMId = dto.FromUoMId,
                    ToUoMId = dto.ToUoMId,
                    Factor = dto.Factor
                });
            }
            await _unitOfWork.SaveChangesAsync();

            var productDto = MapToDto(existedProduct);
            return Result<ProductDto>.Success(productDto);
        }

        public async Task<Result> DeleteAsync(int id, CancellationToken cancellationToken)
        {
            var existed = await _unitOfWork.ProductRepository.GetByIdAsync(id, cancellationToken);
            if(existed == null)
            {
                return Result.Failure($"Product with ID {id} not found.");
            }
            existed.IsDeleted = true;
            await _unitOfWork.ProductRepository.UpdateAsync(existed, cancellationToken);
            await _unitOfWork.SaveChangesAsync(cancellationToken);

            return Result.Success();
        }

        public async Task<Result<bool>> ExistAsync(int id, CancellationToken cancellationToken)
        {
            var exist = await _unitOfWork.ProductRepository.ExistsAsync(p => p.Id == id, cancellationToken);
            return Result<bool>.Success(exist);
        }

        #region Helpers

        private bool IsDataValid(CreateProductDto createProductDto, out string message)
        {
            if (string.IsNullOrWhiteSpace(createProductDto.Name))
            {
                message = "Product name is required.";
                return false;
            }
            if (createProductDto.MinStockLevel < 0)
            {
                message = "Min stock level cannot be negative.";
                return false;
            }
            if (createProductDto.CategoryId <= 0)
            {
                message = "Valid category ID is required.";
                return false;
            }
            if (createProductDto.BaseUoMId <= 0)
            {
                message = "Valid base UoM ID is required.";
                return false;
            }
            message = string.Empty;
            return true;
        }

        private bool IsDataValid(UpdateProductDto updateProductDto, out string message)
        {
            if (string.IsNullOrWhiteSpace(updateProductDto.Name))
            {
                message = "Product name is required.";
                return false;
            }
            if (updateProductDto.MinStockLevel < 0)
            {
                message = "Min stock level cannot be negative.";
                return false;
            }
            if (updateProductDto.CategoryId <= 0)
            {
                message = "Valid category ID is required.";
                return false;
            }
            if (updateProductDto.BaseUoMId <= 0)
            {
                message = "Valid base UoM ID is required.";
                return false;
            }
            message = string.Empty;
            return true;
        }

        private ProductDto MapToDto(Product product)
        {
            return new ProductDto
            {
                Id = product.Id,
                Name = product.Name,
                SKU = product.SKU,
                Barcode = product.Barcode,
                CategoryId = product.CategoryId,
                BaseUoMId = product.BaseUoMId,
                MinStockLevel = product.MinStockLevel,
                IsPerishable = product.IsPerishable,
                IsActive = product.IsActive,
                CreatedAt = product.CreatedAt,
                UpdatedAt = product.UpdatedAt
            };
        }

        #endregion
    }
}
