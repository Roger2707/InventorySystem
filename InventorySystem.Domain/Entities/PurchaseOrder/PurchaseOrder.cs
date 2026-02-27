using InventorySystem.Domain.Enums;

namespace InventorySystem.Domain.Entities.PurchaseOrder
{
    public class PurchaseOrder : BaseEntity
    {
        public string OrderNumber { get; set; }

        public int SupplierId { get; set; }

        public PurchaseOrderStatus Status { get; set; }

        public DateTime OrderDate { get; set; }
        public DateTime? ApprovedDate { get; set; }

        public decimal TotalAmount { get; set; }

        public ICollection<PurchaseOrderLine> Lines { get; set; }
    }
}
