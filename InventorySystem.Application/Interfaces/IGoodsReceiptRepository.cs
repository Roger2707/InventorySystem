using InventorySystem.Domain.Entities.GoodsReceipt;

namespace InventorySystem.Application.Interfaces;

public interface IGoodsReceiptRepository : IRepository<GoodsReceipt>
{
    Task<IEnumerable<GoodsReceipt>> GetAllWithLinesAsync(CancellationToken cancellationToken = default);
    Task<GoodsReceipt?> GetWithLinesAsync(int id, CancellationToken cancellationToken = default);
}

