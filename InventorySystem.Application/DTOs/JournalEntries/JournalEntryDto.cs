namespace InventorySystem.Application.DTOs.JournalEntries
{
    public class JournalEntryDto
    {
        public int Id { get; set; }
        public DateTime PostingDate { get; set; }
        public string Reference { get; set; }
        public int? DeliveryId { get; set; }
        public int? GoodsReceiptId { get; set; }
        public int? InvoiceId { get; set; }
        public ICollection<JournalEntryLineDto> Lines { get; set; } = new List<JournalEntryLineDto>();
    }

    public class JournalEntryLineDto
    {
        public int JournalEntryId { get; set; }
        public int AccountId { get; set; }
        public string AccountType { get; set; }
        public decimal Debit { get; set; }
        public decimal Credit { get; set; }
        public string Description { get; set; }
    }
}
