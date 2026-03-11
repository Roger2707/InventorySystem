using InventorySystem.Domain.Entities.Products;

namespace InventorySystem.Domain.Entities.Delivery
{
    public class DeliveryLine
    {
        public int DeliveryId { get; set; }
        public int ProductId { get; set; }
        public Product Product { get; set; }
        public int RowNumber { get; set; }
        public decimal DeliveredQty { get; set; }
        public decimal InvoicedQty { get; set; } = 0;
        public decimal UnitPrice { get; set; }
        public decimal RemainingInvoicedQty => DeliveredQty - InvoicedQty;
        public decimal LineTotal => DeliveredQty * UnitPrice;
    }
}