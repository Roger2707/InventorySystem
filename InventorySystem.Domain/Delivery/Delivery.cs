using InventorySystem.Domain.Entities;
using InventorySystem.Domain.Enums;

namespace InventorySystem.Domain.Delivery
{
    public class Delivery : BaseEntity
    {
        public int SalesOrderId { get; private set; }

        public DeliveryStatus Status { get; private set; }

        public DateTime DeliveryDate { get; private set; }

        public List<DeliveryLine> Lines { get; private set; } = new();
    }
}
