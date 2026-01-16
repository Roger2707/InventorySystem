using InventorySystem.Domain.Entities.Identity;
using System.Security.Claims;

namespace InventorySystem.Application.Interfaces;

public interface IJwtService
{
    /// <summary>
    /// Generates a JWT token for a user
    /// </summary>
    string GenerateToken(User user, IEnumerable<string> roles, IEnumerable<string> permissions);

    /// <summary>
    /// Validates a JWT token and extracts claims
    /// </summary>
    ClaimsPrincipal? ValidateToken(string token);

    /// <summary>
    /// Gets the expiration time for tokens
    /// </summary>
    DateTime GetTokenExpiration();
}

