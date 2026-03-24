namespace InventorySystem.Application.Interfaces.Generators
{
    public interface IPurchaseOrderGenerator
    {
        Task<string> GenerateAsync(CancellationToken cancellationToken);
    }
}
