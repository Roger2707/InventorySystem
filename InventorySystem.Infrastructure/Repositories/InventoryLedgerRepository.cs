using InventorySystem.Application.Interfaces;
using InventorySystem.Domain.Entities.Inventory;
using InventorySystem.Infrastructure.Data;

namespace InventorySystem.Infrastructure.Repositories;

public class InventoryLedgerRepository : Repository<InventoryLedger>, IInventoryLedgerRepository
{
    public InventoryLedgerRepository(ApplicationDbContext context) : base(context)
    {
    }
}

