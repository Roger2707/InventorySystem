using System.ComponentModel.DataAnnotations.Schema;

namespace InventorySystem.Domain.Entities.Identity
{
    public class User : BaseEntity
    {
        public string Username { get; set; }
        public string PasswordHash { get; set; }
        public string FullName { get; set; }
        public string Email { get; set; }
        public string PhoneNumber { get; set; }
        public string Address { get; set; }

        public bool IsActive { get; set; } = true;

        public int? WarehouseId { get; set; }

        [ForeignKey("WarehouseId")]
        public Warehouse ManagedWarehouse { get; set; }

        // Navigation
        public ICollection<UserRole> UserRoles { get; set; } = new List<UserRole>();
    }
}
