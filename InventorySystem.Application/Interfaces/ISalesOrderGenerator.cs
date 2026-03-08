namespace InventorySystem.Application.Interfaces
{
    public interface ISalesOrderGenerator
    {
        Task<string> GenerateAsync(CancellationToken cancellationToken);
    }
}
