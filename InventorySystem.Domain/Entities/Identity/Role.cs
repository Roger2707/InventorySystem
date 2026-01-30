namespace InventorySystem.Domain.Entities.Identity
{
    public class Role : BaseEntity
    {
        public string RoleName { get; set; }
        public string Description { get; set; }

        // Navigation
        public ICollection<UserRole> UserRoles { get; set; } = new List<UserRole>();
        public ICollection<RolePermission> RolePermissions { get; set; } = new List<RolePermission>();
    }
}
