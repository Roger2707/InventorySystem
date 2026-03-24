using InventorySystem.Application.Common.Pagination;
using InventorySystem.Application.DTOs.Products;
using InventorySystem.Application.Interfaces.Repositories;
using InventorySystem.Domain.Entities.Products;
using InventorySystem.Infrastructure.Data;
using Microsoft.EntityFrameworkCore;

namespace InventorySystem.Infrastructure.Repositories
{
    public class ProductRepository : Repository<Product>, IProductRepository
    {
        public ProductRepository(ApplicationDbContext context) : base(context)
        {
        }
        public async Task<PagedResult<ProductDto>> GetProductsPagedAsync(ProductParams param)
        {
            var query = _context.Products
                                .AsNoTracking()
                                .AsQueryable();

            if (!string.IsNullOrEmpty(param.Name))
            {
                query = query.Where(x => x.Name.Contains(param.Name));
            }

            if (param.MinPrice.HasValue)
            {
                query = query.Where(x => x.BasePrice >= param.MinPrice);
            }

            if (param.MaxPrice.HasValue)
            {
                query = query.Where(x => x.BasePrice <= param.MaxPrice);
            }

            if (param.CategoryIds?.Any() == true)
            {
                query = query.Where(x => param.CategoryIds.Contains(x.CategoryId));
            }

            return await query
                .Select(x => new ProductDto
                {
                    Id = x.Id,
                    Name = x.Name,
                    SKU = x.SKU,
                    Barcode = x.Barcode,
                    CategoryId = x.CategoryId,
                    CategoryName = x.Category.Name,
                    BaseUoMId = x.BaseUoMId,
                    BaseUoMName = x.BaseUoM.Name,
                    BasePrice = x.BasePrice,
                    MinStockLevel = x.MinStockLevel,
                    IsPerishable = x.IsPerishable,
                    IsActive = x.IsActive,
                    CreatedAt = x.CreatedAt,
                    UpdatedAt = x.UpdatedAt,
                    ConversionDtos = x.Conversions.Select(c => new ProductConversionDto
                    {
                        ProductId = x.Id,
                        ProductName = x.Name,
                        UoMIdFrom = c.FromUoMId,
                        UoMNameFrom = c.FromUoM.Name,
                        UoMIdTo = c.ToUoMId,
                        UoMNameTo = c.ToUoM.Name,
                        Factor = c.Factor,
                    }).ToList(),

                })
                .ToPagedResultAsync(param);
        }

        public async Task<Product> GetWithConversionAsync(int id, CancellationToken cancellationToken = default)
        {
            var product = await _dbSet
                .Include(p => p.Conversions)
                .FirstOrDefaultAsync(p => p.Id == id, cancellationToken);

            return product;
        }
    }
}
