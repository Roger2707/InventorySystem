using InventorySystem.Application.Interfaces;
using InventorySystem.Domain.Entities.Products;
using InventorySystem.Infrastructure.Data;

namespace InventorySystem.Infrastructure.Repositories
{
    public class UoMRepository : Repository<UoM>, IUoMRepository
    {
        public UoMRepository(ApplicationDbContext context) : base(context)
        {
        }
    }
}
