namespace InventorySystem.Application.Interfaces
{
    public interface IDeliveryGenerator
    {
        Task<string> GenerateAsync(CancellationToken cancellationToken);
    }
}
