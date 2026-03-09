using InventorySystem.Domain.Enums;

namespace InventorySystem.Domain.Entities.SalesOrder
{
    public class SalesOrder : BaseEntity
    {
        public int CustomerId { get; set; }
        public Customer Customer { get; set; }

        public DateTime OrderDate { get; set; }

        public SalesOrderStatus Status { get; set; }

        public List<SalesOrderLine> Lines { get; set; } = new();
    }
}
