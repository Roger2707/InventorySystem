namespace InventorySystem.Domain.Entities.PurchaseOrder
{
    public class PurchaseOrderLine
    {
        public int PurchaseOrderId { get; private set; }
        public int ProductId { get; private set; }

        public decimal OrderedQty { get; private set; }
        public decimal ReceivedQty { get; private set; }

        public decimal UnitPrice { get; private set; }

        public decimal LineTotal => OrderedQty * UnitPrice;
    }
}
