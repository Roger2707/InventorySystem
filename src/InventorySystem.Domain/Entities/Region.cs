using InventorySystem.Domain.Entities.Identity;

namespace InventorySystem.Domain.Entities
{
    public class Region : BaseEntity
    {
        public string RegionCode { get; set; }
        public string RegionName { get; set; }

        // Navigation
        public ICollection<UserRegion> UserRegions { get; set; } = new List<UserRegion>();
    }
}
