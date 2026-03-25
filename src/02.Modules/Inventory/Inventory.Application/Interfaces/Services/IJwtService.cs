using Inventory.Domain.Entities.Identity;
using System.Security.Claims;

namespace Inventory.Application.Interfaces.Services;

public interface IJwtService
{
    string GenerateToken(User user, IEnumerable<string> roles);
    ClaimsPrincipal ValidateToken(string token);
    DateTime GetTokenExpiration();
}

