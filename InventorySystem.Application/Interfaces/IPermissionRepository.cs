using InventorySystem.Domain.Entities.Identity;

namespace InventorySystem.Application.Interfaces;

public interface IPermissionRepository : IRepository<Permission>
{
    /// <summary>
    /// Gets a permission by permission name
    /// </summary>
    Task<Permission?> GetByPermissionNameAsync(string permissionName, CancellationToken cancellationToken = default);

    /// <summary>
    /// Gets permissions by module
    /// </summary>
    Task<IEnumerable<Permission>> GetByModuleAsync(string module, CancellationToken cancellationToken = default);

    /// <summary>
    /// Gets permissions by module and action
    /// </summary>
    Task<Permission?> GetByModuleAndActionAsync(string module, string action, CancellationToken cancellationToken = default);
}

