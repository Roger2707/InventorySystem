namespace InventorySystem.Application.Interfaces
{
    public interface ISkuGenerator
    {
        Task<string> GenerateAsync(CancellationToken cancellationToken);
    }
}
