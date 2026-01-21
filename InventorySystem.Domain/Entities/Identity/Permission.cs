namespace InventorySystem.Domain.Entities.Identity
{
    public class Permission : BaseEntity
    {
        public string PermissionName { get; set; }
        public string Module { get; set; } // Products, Warehouses, Import, Export...
        public string Action { get; set; } // View, Create, Update, Delete, Approve
        public string Description { get; set; }

        // Navigation
        public ICollection<RolePermission> RolePermissions { get; set; } = new List<RolePermission>();
    }
}
