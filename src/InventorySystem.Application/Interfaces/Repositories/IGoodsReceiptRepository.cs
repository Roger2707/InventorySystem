using InventorySystem.Domain.Entities.GoodsReceipt;

namespace InventorySystem.Application.Interfaces.Repositories;

public interface IGoodsReceiptRepository : IRepository<GoodsReceipt>
{
    Task<IEnumerable<GoodsReceipt>> GetAllWithLinesAsync(CancellationToken cancellationToken = default);
    Task<GoodsReceipt> GetWithLinesAsync(int id, CancellationToken cancellationToken = default);
}

