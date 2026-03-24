using InventorySystem.Domain.Entities.Identity;

namespace InventorySystem.Application.Interfaces.Repositories;

public interface IRoleRepository : IRepository<Role>
{
    Task<Role> GetByRoleNameAsync(string roleName, CancellationToken cancellationToken = default);
    Task<bool> ExistsByRoleNameAsync(string roleName, CancellationToken cancellationToken = default);
    Task<Role> GetWithPermissionsAsync(int roleId, CancellationToken cancellationToken = default);
}

