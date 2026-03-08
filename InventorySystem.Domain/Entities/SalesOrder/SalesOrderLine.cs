namespace InventorySystem.Domain.Entities.SalesOrder
{
    public class SalesOrderLine
    {
        public int SalesOrderId { get; set; }
        public int ProductId { get; private set; }

        public decimal OrderedQty { get; private set; }

        public decimal DeliveredQty { get; private set; }

        public decimal UnitPrice { get; private set; }

        public decimal RemainingQty => OrderedQty - DeliveredQty;
    }
}
