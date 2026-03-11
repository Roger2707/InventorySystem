using InventorySystem.Domain.Enums;

namespace InventorySystem.Domain.Entities.Delivery
{
    public class Delivery : BaseEntity
    {
        public string OrderNumber { get; set; }
        public int SalesOrderId { get; set; }

        public DeliveryStatus Status { get; set; } = DeliveryStatus.Draft;

        public DateTime DeliveryDate { get; set; } = DateTime.UtcNow;
        public decimal TotalAmount { get; set; }

        public List<DeliveryLine> Lines { get; set; } = new();
    }
}
