namespace InventorySystem.Application.Interfaces.Services
{
    public interface IGoodsReceiptGenerator
    {
        Task<string> GenerateAsync(CancellationToken cancellationToken);
    }
}
