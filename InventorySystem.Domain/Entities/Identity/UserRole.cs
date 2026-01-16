namespace InventorySystem.Domain.Entities.Identity
{
    public class UserRole
    {
        public int UserId { get; set; }
        public User User { get; set; }

        public int RoleId { get; set; }
        public Role Role { get; set; }

        public DateTime AssignedAt { get; set; } = DateTime.Now;
    }
}
