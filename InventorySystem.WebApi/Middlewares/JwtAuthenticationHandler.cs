using InventorySystem.Application.Interfaces;
using Microsoft.AspNetCore.Authentication;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using System.Security.Claims;
using System.Text.Encodings.Web;

namespace InventorySystem.WebApi.Middleware;

public class JwtAuthenticationHandler : AuthenticationHandler<AuthenticationSchemeOptions>
{
    private readonly IJwtService _jwtService;

    public JwtAuthenticationHandler(
        IOptionsMonitor<AuthenticationSchemeOptions> options,
        ILoggerFactory logger,
        UrlEncoder encoder,
        IJwtService jwtService)
        : base(options, logger, encoder)
    {
        _jwtService = jwtService;
    }

    protected override async Task<AuthenticateResult> HandleAuthenticateAsync()
    {
        // 1. Check Authorization header exists
        if (!Request.Headers.TryGetValue("Authorization", out var authHeaderValues))
        {
            Logger.LogDebug("Authorization header not found");
            return AuthenticateResult.NoResult();
        }

        var authHeader = authHeaderValues.ToString();

        // 2. Check Bearer scheme
        if (string.IsNullOrEmpty(authHeader) ||
            !authHeader.StartsWith("Bearer ", StringComparison.OrdinalIgnoreCase))
        {
            Logger.LogDebug("Authorization header does not contain Bearer token");
            return AuthenticateResult.NoResult();
        }

        // 3. Extract token
        var token = authHeader.Substring("Bearer ".Length).Trim();

        if (string.IsNullOrEmpty(token))
        {
            Logger.LogWarning("Bearer token is empty");
            return AuthenticateResult.Fail("Token is empty");
        }

        try
        {
            // 4. Validate token
            var principal = _jwtService.ValidateToken(token);

            if (principal == null)
            {
                Logger.LogWarning("Token validation failed: principal is null");
                return AuthenticateResult.Fail("Invalid token");
            }

            // 5. Create authentication ticket
            var ticket = new AuthenticationTicket(principal, Scheme.Name);

            Logger.LogDebug("Token validated successfully for user: {UserId}",
                principal.FindFirst(ClaimTypes.NameIdentifier)?.Value);

            return AuthenticateResult.Success(ticket);
        }
        catch (SecurityTokenExpiredException)
        {
            Logger.LogWarning("Token has expired");
            return AuthenticateResult.Fail("Token has expired");
        }
        catch (SecurityTokenInvalidSignatureException)
        {
            Logger.LogWarning("Token has invalid signature");
            return AuthenticateResult.Fail("Invalid token signature");
        }
        catch (Exception ex)
        {
            Logger.LogError(ex, "Error validating token");
            return AuthenticateResult.Fail($"Token validation error: {ex.Message}");
        }
    }
}

