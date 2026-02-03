namespace InventorySystem.Domain.Entities.Identity
{
    public class UserRegion : BaseEntity
    {
        public int UserId { get; set; }
        public User User { get; set; }

        public int RegionId { get; set; }
        public Region Region { get; set; }
    }
}
