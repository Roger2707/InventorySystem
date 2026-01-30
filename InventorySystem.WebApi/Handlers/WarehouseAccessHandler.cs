using InventorySystem.Application.Services;
using Microsoft.AspNetCore.Authorization;
using System.Security.Claims;

namespace InventorySystem.WebApi.Handlers;

public class WarehouseAccessHandler : AuthorizationHandler<WarehouseAccessRequirement, int>
{
    private readonly IWarehouseService _warehouseService;

    public WarehouseAccessHandler(IWarehouseService warehouseService)
    {
        _warehouseService = warehouseService;
    }

    protected override async Task HandleRequirementAsync(AuthorizationHandlerContext context, WarehouseAccessRequirement requirement, int warehouseId)
    {
        var roles = context.User.Claims
            .Where(c => c.Type == ClaimTypes.Role)
            .Select(c => c.Value)
            .ToList();

        if (roles.Contains("Super_Admin"))
        {
            context.Succeed(requirement);
            return;
        }

        if (!roles.Contains("Warehouse_Manager"))
            return;

        var userId = context.User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
        if (string.IsNullOrEmpty(userId))
            return;

        var result = await _warehouseService.GetByIdAsync(warehouseId);
        if (!result.IsSuccess || result.Data == null)
            return;

        if (result.Data.ManagerId.ToString() == userId)
        {
            context.Succeed(requirement);
        }
    }
}

public class WarehouseAccessRequirement : IAuthorizationRequirement
{

}

