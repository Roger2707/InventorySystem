namespace InventorySystem.Domain.Entities.Baskets
{
    public class Basket : BaseEntity
    {
        public int CustomerId { get; set; }
        public Customer Customer { get; set; } = default!;
        public List<BasketItem> Items { get; set; } = new();
    }
}
