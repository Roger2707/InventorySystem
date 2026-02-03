namespace InventorySystem.Domain.Entities.Identity
{
    public class Permission : BaseEntity
    {
        public string PermissionName { get; set; }
        public string Module { get; set; } // Products, Warehouses
        public string Action { get; set; } // View, Create, Update, Delete, Approve, Import, Export
        public string Description { get; set; }

        // Navigation
        public ICollection<RolePermission> RolePermissions { get; set; } = new List<RolePermission>();
    }
}
