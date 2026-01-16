using Microsoft.AspNetCore.Authorization;

namespace InventorySystem.WebApi.Attributes;

/// <summary>
/// Attribute to require a specific permission
/// Usage: [RequirePermission("Warehouse.Create")]
/// </summary>
public class RequirePermissionAttribute : AuthorizeAttribute
{
    public string Permission { get; }

    public RequirePermissionAttribute(string permission)
    {
        Permission = permission;
        Policy = $"Permission:{permission}";
    }
}
