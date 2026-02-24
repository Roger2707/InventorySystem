using InventorySystem.Application.DTOs.Identity;
using InventorySystem.Domain.Common;

namespace InventorySystem.Application.Interfaces;

public interface IAuthenticationService
{
    Task<Result<AuthResponseDto>> LoginAsync(LoginDto loginDto, CancellationToken cancellationToken = default);
    Task<Result<AuthResponseDto>> RegisterAsync(RegisterDto registerDto, CancellationToken cancellationToken = default);
    Task<Result> ChangePasswordAsync(int userId, ChangePasswordDto changePasswordDto, CancellationToken cancellationToken = default);
}

