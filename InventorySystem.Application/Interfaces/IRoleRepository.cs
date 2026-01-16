using InventorySystem.Domain.Entities.Identity;

namespace InventorySystem.Application.Interfaces;

public interface IRoleRepository : IRepository<Role>
{
    /// <summary>
    /// Gets a role by role name
    /// </summary>
    Task<Role?> GetByRoleNameAsync(string roleName, CancellationToken cancellationToken = default);

    /// <summary>
    /// Checks if a role name already exists
    /// </summary>
    Task<bool> ExistsByRoleNameAsync(string roleName, CancellationToken cancellationToken = default);

    /// <summary>
    /// Gets a role with permissions included
    /// </summary>
    Task<Role?> GetWithPermissionsAsync(int roleId, CancellationToken cancellationToken = default);
}

