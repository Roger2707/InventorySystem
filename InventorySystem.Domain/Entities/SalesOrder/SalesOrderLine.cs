using InventorySystem.Domain.Entities.Products;

namespace InventorySystem.Domain.Entities.SalesOrder
{
    public class SalesOrderLine
    {
        public int SalesOrderId { get; set; }
        public int ProductId { get; set; }
        public Product Product { get; set; }
        public decimal OrderedQty { get; set; }
        public decimal DeliveredQty { get; set; } = 0;
        public decimal UnitPrice { get; set; }
        public decimal RemainingQty => OrderedQty - DeliveredQty;
        public decimal LineTotal => OrderedQty * UnitPrice;
    }
}
