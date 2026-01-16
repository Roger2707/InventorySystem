using InventorySystem.Application.DTOs.Identity;
using InventorySystem.Domain.Common;

namespace InventorySystem.Application.Services;

public interface IUserService
{
    Task<Result<UserDto>> GetByIdAsync(int id, CancellationToken cancellationToken = default);
    Task<Result<IEnumerable<UserDto>>> GetAllAsync(CancellationToken cancellationToken = default);
    Task<Result<UserDto>> CreateAsync(CreateUserDto createDto, CancellationToken cancellationToken = default);
    Task<Result<UserDto>> UpdateAsync(int id, UpdateUserDto updateDto, CancellationToken cancellationToken = default);
    Task<Result> DeleteAsync(int id, CancellationToken cancellationToken = default);
    Task<Result<UserDto>> AssignRolesAsync(int userId, List<int> roleIds, CancellationToken cancellationToken = default);
}

