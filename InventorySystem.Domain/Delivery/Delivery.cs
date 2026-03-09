using InventorySystem.Domain.Entities;
using InventorySystem.Domain.Enums;

namespace InventorySystem.Domain.Delivery
{
    public class Delivery : BaseEntity
    {
        public int SalesOrderId { get; set; }

        public DeliveryStatus Status { get; set; }

        public DateTime DeliveryDate { get; set; }

        public List<DeliveryLine> Lines { get; set; } = new();
    }
}
