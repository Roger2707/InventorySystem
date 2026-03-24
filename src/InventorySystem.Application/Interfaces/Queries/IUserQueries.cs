using InventorySystem.Application.DTOs.Identity;

namespace InventorySystem.Application.Interfaces;

public interface IUserQueries
{
    Task<IReadOnlyList<UserDto>> GetUsersWithRolesAsync(CancellationToken cancellationToken = default);
}

