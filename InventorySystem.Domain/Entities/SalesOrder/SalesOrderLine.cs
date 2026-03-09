using InventorySystem.Domain.Entities.Products;

namespace InventorySystem.Domain.Entities.SalesOrder
{
    public class SalesOrderLine
    {
        public int SalesOrderId { get; set; }
        public int ProductId { get; set; }
        public Product Product { get; set; }
        public decimal OrderedQty { get; set; }

        public decimal DeliveredQty { get; set; }

        public decimal UnitPrice { get; set; }

        public decimal RemainingQty => OrderedQty - DeliveredQty;
    }
}
