using InventorySystem.Application.DTOs.GoodsReceipts;
using InventorySystem.Domain.Common;

namespace InventorySystem.Application.Interfaces.Services;

public interface IGoodsReceiptService
{
    Task<Result<IEnumerable<GoodsReceiptDto>>> GetAllAsync(CancellationToken cancellationToken = default);
    Task<Result<GoodsReceiptDto>> GetByIdAsync(int id, CancellationToken cancellationToken = default);
}

