using InventorySystem.Domain.Enums;

namespace InventorySystem.Domain.Entities.GoodsReceipt
{
    public class GoodsReceipt : BaseEntity
    {
        public string ReceiptNumber { get; private set; }

        public int PurchaseOrderId { get; private set; }

        public int WarehouseId { get; private set; }

        public ReceiptStatus Status { get; private set; }

        public DateTime ReceiptDate { get; private set; }

        public ICollection<GoodsReceiptLine> Lines { get; private set; }
    }
}
