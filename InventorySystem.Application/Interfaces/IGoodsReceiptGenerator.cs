namespace InventorySystem.Application.Interfaces
{
    public interface IGoodsReceiptGenerator
    {
        Task<string> GenerateAsync(CancellationToken cancellationToken);
    }
}
