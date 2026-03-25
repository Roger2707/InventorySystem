using Inventory.Application.Interfaces.Repositories;
using Inventory.Domain.Entities.Identity;
using Inventory.Infrastructure.Data;
using Microsoft.EntityFrameworkCore;

namespace Inventory.Infrastructure.Repositories.Identity;

public class RoleRepository : Repository<Role>, IRoleRepository
{
    public RoleRepository(ApplicationDbContext context)
        : base(context)
    {
    }

    public async Task<Role?> GetByRoleNameAsync(string roleName, CancellationToken cancellationToken = default)
    {
        return await _dbSet
            .FirstOrDefaultAsync(r => r.RoleName == roleName, cancellationToken);
    }

    public async Task<bool> ExistsByRoleNameAsync(string roleName, CancellationToken cancellationToken = default)
    {
        return await _dbSet
            .AnyAsync(r => r.RoleName == roleName, cancellationToken);
    }

    public async Task<Role?> GetWithPermissionsAsync(int roleId, CancellationToken cancellationToken = default)
    {
        return await _dbSet
            .Include(r => r.RolePermissions)
                .ThenInclude(rp => rp.Permission)
            .FirstOrDefaultAsync(r => r.Id == roleId, cancellationToken);
    }
}

