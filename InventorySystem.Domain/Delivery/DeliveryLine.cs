namespace InventorySystem.Domain.Delivery
{
    public class DeliveryLine
    {
        public int DeliveryId { get; set; }
        public int ProductId { get; set; }
        public decimal Quantity { get; set; }
    }
}