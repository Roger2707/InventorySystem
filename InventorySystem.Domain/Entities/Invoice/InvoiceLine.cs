namespace InventorySystem.Domain.Entities.Invoice
{
    public class InvoiceLine : BaseEntity
    {
        public int InvoiceId { get; set; }
        public int DeliveryLineId { get; set; }
        public decimal Quantity { get; set; }
        public decimal UnitPrice { get; set; }
        public decimal LineTotal => Quantity * UnitPrice;
    }
}