using InventorySystem.Application.Interfaces;

namespace InventorySystem.Application.Services;

public class AuthorizationService : IAuthorizationService
{
    private readonly IUnitOfWork _unitOfWork;

    public AuthorizationService(IUnitOfWork unitOfWork)
    {
        _unitOfWork = unitOfWork;
    }

    public async Task<bool> HasPermissionAsync(int userId, string permissionName, CancellationToken cancellationToken = default)
    {
        var user = await _unitOfWork.UserRepository.GetWithRolesAndPermissionsAsync(userId, cancellationToken);

        if (user == null || !user.IsActive)
        {
            return false;
        }

        var hasPermission = user.UserRoles
            .SelectMany(ur => ur.Role.RolePermissions)
            .Any(rp => rp.Permission.PermissionName == permissionName);

        return hasPermission;
    }

    public async Task<bool> HasPermissionAsync(int userId, string module, string action, CancellationToken cancellationToken = default)
    {
        var user = await _unitOfWork.UserRepository.GetWithRolesAndPermissionsAsync(userId, cancellationToken);

        if (user == null || !user.IsActive)
        {
            return false;
        }

        var hasPermission = user.UserRoles
            .SelectMany(ur => ur.Role.RolePermissions)
            .Any(rp => rp.Permission.Module == module && rp.Permission.Action == action);

        return hasPermission;
    }

    public async Task<IEnumerable<string>> GetUserPermissionsAsync(int userId, CancellationToken cancellationToken = default)
    {
        var user = await _unitOfWork.UserRepository.GetWithRolesAndPermissionsAsync(userId, cancellationToken);

        if (user == null || !user.IsActive)
        {
            return Enumerable.Empty<string>();
        }

        return user.UserRoles
            .SelectMany(ur => ur.Role.RolePermissions)
            .Select(rp => rp.Permission.PermissionName)
            .Distinct()
            .ToList();
    }

    public async Task<IEnumerable<string>> GetUserRolesAsync(int userId, CancellationToken cancellationToken = default)
    {
        var user = await _unitOfWork.UserRepository.GetWithRolesAndPermissionsAsync(userId, cancellationToken);

        if (user == null || !user.IsActive)
        {
            return Enumerable.Empty<string>();
        }

        return user.UserRoles
            .Select(ur => ur.Role.RoleName)
            .Distinct()
            .ToList();
    }
}

