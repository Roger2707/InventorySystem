using InventorySystem.Domain.Enums;

namespace InventorySystem.Domain.Entities.Invoice
{
    public class Invoice : BaseEntity
    {
        public int SalesOrderId { get; set; }
        public int DeliveryId { get; set; }
        public string InvoiceNumber { get; set; } = string.Empty;
        public DateTime InvoiceDate { get; set; }
        public decimal TotalAmount { get; set; }
        public InvoiceStatus Status { get; set; }
        public ICollection<InvoiceLine> Lines { get; set; }
    }
}
