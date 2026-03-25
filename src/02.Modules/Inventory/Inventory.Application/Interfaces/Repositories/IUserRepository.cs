using Inventory.Domain.Entities.Identity;

namespace Inventory.Application.Interfaces.Repositories;

public interface IUserRepository : IRepository<User>
{
    Task<User> GetByUsernameAsync(string username, CancellationToken cancellationToken = default);
    Task<User> GetByEmailAsync(string email, CancellationToken cancellationToken = default);
    Task<bool> ExistsByUsernameAsync(string username, CancellationToken cancellationToken = default);
    Task<bool> ExistsByEmailAsync(string email, CancellationToken cancellationToken = default);
    Task<User> GetWithRolesAsync(int userId, CancellationToken cancellationToken = default);
    Task<IEnumerable<User>> GetActiveUsersAsync(CancellationToken cancellationToken = default);
}

