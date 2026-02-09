using Microsoft.AspNetCore.Authorization;

namespace InventorySystem.WebApi.Policies
{
    public class WarehousePermissionHandler : AuthorizationHandler<WarehousePermissionRequirement>
    {
        protected override Task HandleRequirementAsync(AuthorizationHandlerContext context, WarehousePermissionRequirement requirement)
        {
            throw new NotImplementedException();
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
