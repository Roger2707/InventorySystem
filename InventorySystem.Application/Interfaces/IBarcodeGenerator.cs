namespace InventorySystem.Application.Interfaces
{
    public interface IBarcodeGenerator
    {
        Task<string> GenerateAsync(CancellationToken cancellationToken);
    }
}
