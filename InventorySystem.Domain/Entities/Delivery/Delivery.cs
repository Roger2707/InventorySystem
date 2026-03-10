using InventorySystem.Domain.Entities;
using InventorySystem.Domain.Enums;

namespace InventorySystem.Domain.Entities.Delivery
{
    public class Delivery : BaseEntity
    {
        public string OrderNumber { get; set; }
        public int SalesOrderId { get; set; }

        public DeliveryStatus Status { get; set; }

        public DateTime DeliveryDate { get; set; }
        public decimal TotalAmount { get; set; }

        public List<DeliveryLine> Lines { get; set; } = new();
    }
}
