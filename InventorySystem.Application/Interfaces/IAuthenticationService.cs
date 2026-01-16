using InventorySystem.Application.DTOs.Identity;
using InventorySystem.Domain.Common;

namespace InventorySystem.Application.Interfaces;

public interface IAuthenticationService
{
    /// <summary>
    /// Authenticates a user and returns a JWT token
    /// </summary>
    Task<Result<AuthResponseDto>> LoginAsync(LoginDto loginDto, CancellationToken cancellationToken = default);

    /// <summary>
    /// Registers a new user
    /// </summary>
    Task<Result<AuthResponseDto>> RegisterAsync(RegisterDto registerDto, CancellationToken cancellationToken = default);

    /// <summary>
    /// Changes a user's password
    /// </summary>
    Task<Result> ChangePasswordAsync(int userId, ChangePasswordDto changePasswordDto, CancellationToken cancellationToken = default);
}

