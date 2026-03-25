using Inventory.Domain.Entities.Identity;

namespace Inventory.Application.Interfaces.Repositories;

public interface IPermissionRepository : IRepository<Permission>
{
    Task<Permission> GetByPermissionNameAsync(string permissionName, CancellationToken cancellationToken = default);
    Task<IEnumerable<Permission>> GetByModuleAsync(string module, CancellationToken cancellationToken = default);
    Task<Permission> GetByModuleAndActionAsync(string module, string action, CancellationToken cancellationToken = default);
}

