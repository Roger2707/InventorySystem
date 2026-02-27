using InventorySystem.Application.Interfaces;
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

        public async Task<Product> GetWithConversionAsync(int id, CancellationToken cancellationToken = default)
        {
            var product = await _dbSet
                .Include(p => p.Conversions)
                .FirstOrDefaultAsync(p => p.Id == id, cancellationToken);

            return product;
        }
    }
}
