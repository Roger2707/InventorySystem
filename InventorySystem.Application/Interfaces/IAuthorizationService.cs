namespace InventorySystem.Application.Interfaces;

public interface IAuthorizationService
{
    Task<bool> HasPermissionAsync(int userId, string permissionName, CancellationToken cancellationToken = default);
    Task<bool> HasPermissionAsync(int userId, string module, string action, CancellationToken cancellationToken = default);
    Task<IEnumerable<string>> GetUserPermissionsAsync(int userId, CancellationToken cancellationToken = default);
    Task<IEnumerable<string>> GetUserRolesAsync(int userId, CancellationToken cancellationToken = default);
}

