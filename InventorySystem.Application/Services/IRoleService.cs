using InventorySystem.Application.DTOs.Identity;
using InventorySystem.Domain.Common;

namespace InventorySystem.Application.Services;

public interface IRoleService
{
    Task<Result<RoleDto>> GetByIdAsync(int id, CancellationToken cancellationToken = default);
    Task<Result<IEnumerable<RoleDto>>> GetAllAsync(CancellationToken cancellationToken = default);
    Task<Result<RoleDto>> CreateAsync(CreateRoleDto createDto, CancellationToken cancellationToken = default);
    Task<Result<RoleDto>> UpdateAsync(int id, UpdateRoleDto updateDto, CancellationToken cancellationToken = default);
    Task<Result> DeleteAsync(int id, CancellationToken cancellationToken = default);
    Task<Result<RoleDto>> AssignPermissionsAsync(int roleId, List<int> permissionIds, CancellationToken cancellationToken = default);
}

