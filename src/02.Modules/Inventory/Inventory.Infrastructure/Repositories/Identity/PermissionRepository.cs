using Inventory.Application.Interfaces.Repositories;
using Inventory.Domain.Entities.Identity;
using Inventory.Infrastructure.Data;
using Microsoft.EntityFrameworkCore;

namespace Inventory.Infrastructure.Repositories.Identity;

public class PermissionRepository : Repository<Permission>, IPermissionRepository
{
    public PermissionRepository(ApplicationDbContext context)
        : base(context)
    {
    }

    public async Task<Permission?> GetByPermissionNameAsync(string permissionName, CancellationToken cancellationToken = default)
    {
        return await _dbSet
            .FirstOrDefaultAsync(p => p.PermissionName == permissionName, cancellationToken);
    }

    public async Task<IEnumerable<Permission>> GetByModuleAsync(string module, CancellationToken cancellationToken = default)
    {
        return await _dbSet
            .Where(p => p.Module == module)
            .OrderBy(p => p.Action)
            .ToListAsync(cancellationToken);
    }

    public async Task<Permission?> GetByModuleAndActionAsync(string module, string action, CancellationToken cancellationToken = default)
    {
        return await _dbSet
            .FirstOrDefaultAsync(p => p.Module == module && p.Action == action, cancellationToken);
    }
}

