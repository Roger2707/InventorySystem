using Inventory.Application.DTOs.Identity;
using Inventory.Domain.Entities.Identity;
using SharedKernel;

namespace Inventory.Application.Interfaces;

public interface IUserService
{
    Task<Result<UserDto>> GetByIdAsync(int id, CancellationToken cancellationToken = default);
    Task<Result<IEnumerable<UserDto>>> GetAllAsync(CancellationToken cancellationToken = default);
    Task<Result<UserDto>> CreateAsync(CreateUserDto createDto, CancellationToken cancellationToken = default);
    Task<Result<UserDto>> UpdateAsync(int id, UpdateUserDto updateDto, CancellationToken cancellationToken = default);
    Task<Result> DeleteAsync(int id, CancellationToken cancellationToken = default);
    Task<Result<UserDto>> AssignRolesAsync(int userId, List<int> roleIds, CancellationToken cancellationToken = default);
    Task<Result<bool>> IsExistedUserRegion(int userId, int regionId, CancellationToken cancellationToken = default);
    Task<Result<UserWarehouse>> GetUserWarehouseAsync(int userId, int warehouseId, CancellationToken cancellationToken = default);
}



