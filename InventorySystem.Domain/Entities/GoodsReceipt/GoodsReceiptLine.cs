namespace InventorySystem.Domain.Entities.GoodsReceipt
{
    public class GoodsReceiptLine : BaseEntity
    {
        public int GoodsReceiptId { get; private set; }
        public int PurchaseOrderLineId { get; private set; }

        public int ProductId { get; private set; }

        public decimal ReceivedQty { get; private set; }

        // Giá thực tế dùng cho FIFO
        public decimal UnitCost { get; private set; }

        public decimal LineTotal => ReceivedQty * UnitCost;
    }
}
