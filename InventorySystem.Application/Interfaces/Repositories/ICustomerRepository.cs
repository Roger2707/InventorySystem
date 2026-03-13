using InventorySystem.Domain.Entities;

namespace InventorySystem.Application.Interfaces.Repositories;

public interface ICustomerRepository : IRepository<Customer>
{
    Task<Customer> GetByCodeAsync(string customerCode, CancellationToken cancellationToken = default);
    Task<bool> ExistsByCodeAsync(string customerCode, CancellationToken cancellationToken = default);
    Task<List<string>> GetAllCustomerCodeAsync(CancellationToken cancellationToken);
}

