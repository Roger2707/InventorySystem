using Inventory.Application.DTOs.Identity;

namespace Inventory.Application.Interfaces;

public interface IUserQueries
{
    Task<IReadOnlyList<UserDto>> GetUsersWithRolesAsync(CancellationToken cancellationToken = default);
}

