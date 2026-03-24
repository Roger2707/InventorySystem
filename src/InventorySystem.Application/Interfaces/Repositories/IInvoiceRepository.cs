using InventorySystem.Domain.Entities.Invoice;

namespace InventorySystem.Application.Interfaces.Repositories
{
    public interface IInvoiceRepository : IRepository<Invoice>
    {
        Task<IEnumerable<Invoice>> GetAllWithLinesAsync(CancellationToken cancellationToken = default);
        Task<Invoice> GetWithLinesAsync(int id, CancellationToken cancellationToken = default);
    }
}
