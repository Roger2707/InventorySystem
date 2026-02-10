using InventorySystem.Domain.Entities.Identity;
using System.Security.Claims;

namespace InventorySystem.Application.Interfaces;

public interface IJwtService
{
    string GenerateToken(User user, IEnumerable<string> roles);
    ClaimsPrincipal? ValidateToken(string token);
    DateTime GetTokenExpiration();
}

