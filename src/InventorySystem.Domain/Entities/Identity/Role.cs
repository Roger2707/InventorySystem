namespace InventorySystem.Domain.Entities.Identity
{
    public class Role : BaseEntity
    {
        public string RoleName { get; set; }
        public string Description { get; set; }
        public RoleLevel RoleLevel { get; set; } = RoleLevel.Staff;

        // Navigation
        public ICollection<UserRole> UserRoles { get; set; } = new List<UserRole>();
        public ICollection<RolePermission> RolePermissions { get; set; } = new List<RolePermission>();
    }

    public enum RoleLevel
    {
        SuperAdmin = 4,
        RegionalManager = 3,
        WarehouseManager = 2,
        Staff = 1
    }
}
