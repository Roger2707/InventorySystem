using InventorySystem.Application.DTOs.Identity;
using InventorySystem.Application.Interfaces;
using InventorySystem.Domain.Common;
using InventorySystem.Domain.Entities.Identity;

namespace InventorySystem.Application.Services;

public class RoleService : IRoleService
{
    private readonly IUnitOfWork _unitOfWork;

    public RoleService(IUnitOfWork unitOfWork)
    {
        _unitOfWork = unitOfWork;
    }

    public async Task<Result<RoleDto>> GetByIdAsync(int id, CancellationToken cancellationToken = default)
    {
        var role = await _unitOfWork.RoleRepository.GetWithPermissionsAsync(id, cancellationToken);

        if (role == null)
        {
            return Result<RoleDto>.Failure($"Role with ID {id} not found.");
        }

        var permissions = role.RolePermissions
            .Select(rp => rp.Permission.PermissionName)
            .Distinct()
            .ToList();

        var roleDto = MapToDto(role, permissions);
        return Result<RoleDto>.Success(roleDto);
    }

    public async Task<Result<IEnumerable<RoleDto>>> GetAllAsync(CancellationToken cancellationToken = default)
    {
        var roles = await _unitOfWork.RoleRepository.GetAllAsync(cancellationToken);
        var roleDtos = new List<RoleDto>();

        foreach (var role in roles)
        {
            var roleWithPermissions = await _unitOfWork.RoleRepository.GetWithPermissionsAsync(role.Id, cancellationToken);
            if (roleWithPermissions != null)
            {
                var permissions = roleWithPermissions.RolePermissions
                    .Select(rp => rp.Permission.PermissionName)
                    .Distinct()
                    .ToList();
                roleDtos.Add(MapToDto(roleWithPermissions, permissions));
            }
        }

        return Result<IEnumerable<RoleDto>>.Success(roleDtos);
    }

    public async Task<Result<RoleDto>> CreateAsync(CreateRoleDto createDto, CancellationToken cancellationToken = default)
    {
        // Check if role name already exists
        if (await _unitOfWork.RoleRepository.ExistsByRoleNameAsync(createDto.RoleName, cancellationToken))
        {
            return Result<RoleDto>.Failure($"Role with name '{createDto.RoleName}' already exists.");
        }

        // Create role
        var role = new Role
        {
            RoleName = createDto.RoleName,
            Description = createDto.Description
        };

        await _unitOfWork.RoleRepository.AddAsync(role, cancellationToken);
        await _unitOfWork.SaveChangesAsync(cancellationToken);

        // Assign permissions
        if (createDto.PermissionIds.Any())
        {
            await AssignPermissionsInternalAsync(role.Id, createDto.PermissionIds, cancellationToken);
        }

        var createdRole = await _unitOfWork.RoleRepository.GetWithPermissionsAsync(role.Id, cancellationToken);
        if (createdRole == null)
        {
            return Result<RoleDto>.Failure("Failed to create role.");
        }

        var permissions = createdRole.RolePermissions
            .Select(rp => rp.Permission.PermissionName)
            .Distinct()
            .ToList();

        var roleDto = MapToDto(createdRole, permissions);
        return Result<RoleDto>.Success(roleDto);
    }

    public async Task<Result<RoleDto>> UpdateAsync(int id, UpdateRoleDto updateDto, CancellationToken cancellationToken = default)
    {
        var role = await _unitOfWork.RoleRepository.GetByIdAsync(id, cancellationToken);

        if (role == null)
        {
            return Result<RoleDto>.Failure($"Role with ID {id} not found.");
        }

        // Update role properties
        if (!string.IsNullOrEmpty(updateDto.Description))
            role.Description = updateDto.Description;

        await _unitOfWork.RoleRepository.UpdateAsync(role, cancellationToken);

        // Update permissions if provided
        if (updateDto.PermissionIds != null)
        {
            await AssignPermissionsInternalAsync(id, updateDto.PermissionIds, cancellationToken);
        }

        await _unitOfWork.SaveChangesAsync(cancellationToken);

        var updatedRole = await _unitOfWork.RoleRepository.GetWithPermissionsAsync(id, cancellationToken);
        if (updatedRole == null)
        {
            return Result<RoleDto>.Failure("Failed to update role.");
        }

        var permissions = updatedRole.RolePermissions
            .Select(rp => rp.Permission.PermissionName)
            .Distinct()
            .ToList();

        var roleDto = MapToDto(updatedRole, permissions);
        return Result<RoleDto>.Success(roleDto);
    }

    public async Task<Result> DeleteAsync(int id, CancellationToken cancellationToken = default)
    {
        var role = await _unitOfWork.RoleRepository.GetByIdAsync(id, cancellationToken);

        if (role == null)
        {
            return Result.Failure($"Role with ID {id} not found.");
        }

        // Soft delete
        role.IsDeleted = true;
        await _unitOfWork.RoleRepository.UpdateAsync(role, cancellationToken);
        await _unitOfWork.SaveChangesAsync(cancellationToken);

        return Result.Success();
    }

    public async Task<Result<RoleDto>> AssignPermissionsAsync(int roleId, List<int> permissionIds, CancellationToken cancellationToken = default)
    {
        var role = await _unitOfWork.RoleRepository.GetByIdAsync(roleId, cancellationToken);

        if (role == null)
        {
            return Result<RoleDto>.Failure($"Role with ID {roleId} not found.");
        }

        await AssignPermissionsInternalAsync(roleId, permissionIds, cancellationToken);
        await _unitOfWork.SaveChangesAsync(cancellationToken);

        var updatedRole = await _unitOfWork.RoleRepository.GetWithPermissionsAsync(roleId, cancellationToken);
        if (updatedRole == null)
        {
            return Result<RoleDto>.Failure("Failed to assign permissions.");
        }

        var permissions = updatedRole.RolePermissions
            .Select(rp => rp.Permission.PermissionName)
            .Distinct()
            .ToList();

        var roleDto = MapToDto(updatedRole, permissions);
        return Result<RoleDto>.Success(roleDto);
    }

    private async Task AssignPermissionsInternalAsync(int roleId, List<int> permissionIds, CancellationToken cancellationToken)
    {
        var role = await _unitOfWork.RoleRepository.GetWithPermissionsAsync(roleId, cancellationToken);
        if (role == null) return;

        // Remove existing permissions
        var existingRolePermissions = role.RolePermissions.ToList();
        foreach (var rolePermission in existingRolePermissions)
        {
            var rolePermissionEntity = await _unitOfWork.GetRepository<RolePermission>()
                .FirstOrDefaultAsync(rp => rp.RoleId == roleId && rp.PermissionId == rolePermission.PermissionId, cancellationToken);
            if (rolePermissionEntity != null)
            {
                await _unitOfWork.GetRepository<RolePermission>().DeleteAsync(rolePermissionEntity, cancellationToken);
            }
        }

        // Add new permissions
        foreach (var permissionId in permissionIds)
        {
            var permission = await _unitOfWork.PermissionRepository.GetByIdAsync(permissionId, cancellationToken);
            if (permission != null)
            {
                var rolePermission = new RolePermission
                {
                    RoleId = roleId,
                    PermissionId = permissionId,
                    GrantedAt = DateTime.UtcNow
                };
                await _unitOfWork.GetRepository<RolePermission>().AddAsync(rolePermission, cancellationToken);
            }
        }
    }

    private static RoleDto MapToDto(Role role, List<string> permissions)
    {
        return new RoleDto
        {
            Id = role.Id,
            RoleName = role.RoleName,
            Description = role.Description,
            CreatedAt = role.CreatedAt,
            UpdatedAt = role.UpdatedAt,
            Permissions = permissions
        };
    }
}

