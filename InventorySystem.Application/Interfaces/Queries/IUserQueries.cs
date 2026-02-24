using InventorySystem.Application.DTOs.Identity;

namespace InventorySystem.Application.Interfaces;

/// <summary>
/// Read-only queries (Dapper-friendly) for complex SELECT/JOIN scenarios.
/// </summary>
public interface IUserQueries
{
    Task<IReadOnlyList<UserDto>> GetUsersWithRolesAsync(CancellationToken cancellationToken = default);
}

