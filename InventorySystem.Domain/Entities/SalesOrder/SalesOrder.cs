using InventorySystem.Domain.Enums;

namespace InventorySystem.Domain.Entities.SalesOrder
{
    public class SalesOrder : BaseEntity
    {
        public string OrderNumber { get; set; }
        public int CustomerId { get; set; }
        public Customer Customer { get; set; }
        public DateTime OrderDate { get; set; }
        public SalesOrderStatus Status { get; set; } = SalesOrderStatus.Draft;
        public decimal TotalAmount { get; set; }
        public List<SalesOrderLine> Lines { get; set; } = new();
    }
}
