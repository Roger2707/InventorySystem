using InventorySystem.Application.Interfaces.Repositories;
using InventorySystem.Domain.Entities.StockTransfer;
using InventorySystem.Infrastructure.Data;

namespace InventorySystem.Infrastructure.Repositories;

public class StockTransferRepository : Repository<StockTransfer>, IStockTransferRepository
{
    public StockTransferRepository(ApplicationDbContext context) : base(context)
    {
    }
}

