using InventorySystem.Application.Extensions;
using InventorySystem.Application.Services;
using Microsoft.AspNetCore.Authorization;
using System.Security.Claims;

namespace InventorySystem.WebApi.Policies
{
    public class WarehousePermissionHandler : AuthorizationHandler<WarehousePermissionRequirement>
    {
        private readonly IHttpContextAccessor _httpContextAccessor;
        private readonly IPermissionService _permissionService;
        private readonly IWarehouseService _warehouseService;

        public WarehousePermissionHandler(IHttpContextAccessor httpContextAccessor, IPermissionService permissionService, IWarehouseService warehouseService)
        {
            _httpContextAccessor = httpContextAccessor;
            _permissionService = permissionService;
            _warehouseService = warehouseService;
        }

        protected override async Task HandleRequirementAsync(AuthorizationHandlerContext context, WarehousePermissionRequirement requirement)
        {
            int userId = CF.GetInt(context.User.FindFirst(ClaimTypes.NameIdentifier)?.Value);
            if(userId == 0)
            {
                context.Fail();
                return;
            }

            int warehouseId = GetWarehouseIdFromRequest() ?? 0;
            if (warehouseId == 0)
            {
                context.Fail();
                return;
            }

            if (context.User.IsInRole("Super_Admin"))
            {
                context.Succeed(requirement);
                return;
            }

            if(context.User.IsInRole("Regional_Manager"))
            {
                var regionIdResult = await _warehouseService.GetRegionIdByWarehouseIdAsync(warehouseId);
                if (!regionIdResult.IsSuccess)
                {
                    context.Fail();
                    return;
                }
                int regionId = regionIdResult.Data;

            }


            var hasPermissionResult = await _permissionService
                .CanRoleHasPermissionAsync(userId, requirement.Module, requirement.Action);

            if (hasPermissionResult.IsSuccess)
            {
                context.Succeed(requirement);
            }
            else
            {
                context.Fail();
            }
        }

        private int? GetWarehouseIdFromRequest()
        {
            var httpContext = _httpContextAccessor.HttpContext;
            if (httpContext == null)
                return null;

            // Try route values first
            if (httpContext.Request.RouteValues.TryGetValue("warehouseId", out var routeValue))
            {
                if (int.TryParse(routeValue?.ToString(), out int warehouseId))
                    return warehouseId;
            }

            // Try query string
            if (httpContext.Request.Query.TryGetValue("warehouseId", out var queryValue))
            {
                if (int.TryParse(queryValue.ToString(), out int warehouseId))
                    return warehouseId;
            }

            return null;
        }
    }

    public class WarehousePermissionRequirement : IAuthorizationRequirement
    {
        public string Module { get; }
        public string Action { get; }

        public WarehousePermissionRequirement(string module, string action)
        {
            Module = module;
            Action = action;
        }
    }
}
