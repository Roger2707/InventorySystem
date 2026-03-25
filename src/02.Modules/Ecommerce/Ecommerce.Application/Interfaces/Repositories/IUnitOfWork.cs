namespace ECommerce.Application.Interfaces.Repositories
{
    public interface IUnitOfWork : IDisposable
    {
        IBasketRepository BasketRepository { get; }
        Task<int> SaveChangesAsync(CancellationToken cancellationToken = default);
    }
}
