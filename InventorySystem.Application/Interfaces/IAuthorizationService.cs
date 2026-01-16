using System.Security.Claims;

namespace InventorySystem.Application.Interfaces;

public interface IAuthorizationService
{
    /// <summary>
    /// Checks if a user has a specific permission
    /// </summary>
    Task<bool> HasPermissionAsync(int userId, string permissionName, CancellationToken cancellationToken = default);

    /// <summary>
    /// Checks if a user has a permission for a specific module and action
    /// </summary>
    Task<bool> HasPermissionAsync(int userId, string module, string action, CancellationToken cancellationToken = default);

    /// <summary>
    /// Gets all permissions for a user
    /// </summary>
    Task<IEnumerable<string>> GetUserPermissionsAsync(int userId, CancellationToken cancellationToken = default);

    /// <summary>
    /// Gets all roles for a user
    /// </summary>
    Task<IEnumerable<string>> GetUserRolesAsync(int userId, CancellationToken cancellationToken = default);
}

