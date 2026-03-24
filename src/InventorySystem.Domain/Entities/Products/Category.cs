namespace InventorySystem.Domain.Entities.Products
{
    public class Category : BaseEntity
    {
        public string Name { get; set; }
        public int? ParentId { get; set; }
    }
}
