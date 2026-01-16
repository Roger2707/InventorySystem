using InventorySystem.Domain.Entities.Identity;

namespace InventorySystem.Application.Interfaces;

public interface IUserRepository : IRepository<User>
{
    /// <summary>
    /// Gets a user by username
    /// </summary>
    Task<User?> GetByUsernameAsync(string username, CancellationToken cancellationToken = default);

    /// <summary>
    /// Gets a user by email
    /// </summary>
    Task<User?> GetByEmailAsync(string email, CancellationToken cancellationToken = default);

    /// <summary>
    /// Checks if a username already exists
    /// </summary>
    Task<bool> ExistsByUsernameAsync(string username, CancellationToken cancellationToken = default);

    /// <summary>
    /// Checks if an email already exists
    /// </summary>
    Task<bool> ExistsByEmailAsync(string email, CancellationToken cancellationToken = default);

    /// <summary>
    /// Gets a user with roles and permissions included
    /// </summary>
    Task<User?> GetWithRolesAndPermissionsAsync(int userId, CancellationToken cancellationToken = default);

    /// <summary>
    /// Gets all active users
    /// </summary>
    Task<IEnumerable<User>> GetActiveUsersAsync(CancellationToken cancellationToken = default);
}

