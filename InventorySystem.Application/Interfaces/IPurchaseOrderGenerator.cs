namespace InventorySystem.Application.Interfaces
{
    public interface IPurchaseOrderGenerator
    {
        Task<string> GenerateAsync(CancellationToken cancellationToken);
    }
}
