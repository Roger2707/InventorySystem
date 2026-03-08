using InventorySystem.Domain.Enums;

namespace InventorySystem.Domain.Entities.SalesOrder
{
    public class SalesOrder : BaseEntity
    {
        public int CustomerId { get; private set; }

        public DateTime OrderDate { get; private set; }

        public SalesOrderStatus Status { get; private set; }

        public List<SalesOrderLine> Lines { get; private set; } = new();
    }
}
