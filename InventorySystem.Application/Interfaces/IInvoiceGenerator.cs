namespace InventorySystem.Application.Interfaces
{
    public interface IInvoiceGenerator
    {
        Task<string> GenerateAsync(CancellationToken cancellationToken);
    }
}
