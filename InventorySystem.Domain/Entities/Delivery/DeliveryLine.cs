using InventorySystem.Domain.Entities.Products;

namespace InventorySystem.Domain.Entities.Delivery
{
    public class DeliveryLine
    {
        public int DeliveryId { get; set; }
        public int ProductId { get; set; }
        public Product Product { get; set; }
        public decimal Quantity { get; set; }
        public decimal UnitPrice { get; set; }
        public decimal LineTotal => Quantity * UnitPrice;
    }
}