namespace InventorySystem.Domain.Entities.Products
{
    public class UoM : BaseEntity
    {
        public string Name { get; set; } = default!;
        // Navigation
        public ICollection<Product> Products { get; set; } = new List<Product>();
    }
}
