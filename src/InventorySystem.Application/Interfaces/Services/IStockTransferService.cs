using InventorySystem.Domain.Common;
using InventorySystem.Domain.Entities.StockTransfer;

namespace InventorySystem.Application.Interfaces.Services;

public interface IStockTransferService
{
    Task<Result<IEnumerable<StockTransfer>>> GetAllAsync(CancellationToken cancellationToken = default);
    Task<Result<StockTransfer>> GetByIdAsync(int id, CancellationToken cancellationToken = default);
}

