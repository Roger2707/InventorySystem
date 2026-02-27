using InventorySystem.Application.Interfaces;
using InventorySystem.Domain.Entities.Inventory;
using InventorySystem.Infrastructure.Data;

namespace InventorySystem.Infrastructure.Repositories;

public class InventoryCostLayerRepository : Repository<InventoryCostLayer>, IInventoryCostLayerRepository
{
    public InventoryCostLayerRepository(ApplicationDbContext context) : base(context)
    {
    }
}

