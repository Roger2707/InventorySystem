namespace InventorySystem.Domain.Delivery
{
    public class DeliveryLine
    {
        public int DeliveryId { get; set; }
        public int ProductId { get; private set; }
        public decimal Quantity { get; private set; }
    }
}