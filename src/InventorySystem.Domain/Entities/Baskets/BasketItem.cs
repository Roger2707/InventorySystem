using InventorySystem.Domain.Entities.Products;

namespace InventorySystem.Domain.Entities.Baskets
{
    public class BasketItem
    {
        public int BasketId { get; set; }
        public int ProductId { get; set; }
        public Product Product { get; set; } = default!;
        public int RowNumber { get; set; }
        public decimal Quantity { get; set; }
        public decimal UnitPrice { get; set; }
        public decimal LineTotal => Quantity * UnitPrice;
    }
}
