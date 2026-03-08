namespace InventorySystem.Domain.Entities.Inventory
{
    public class InventoryReservation : BaseEntity
    {
        public int ProductId { get; set; }

        public decimal ReservedQty { get; set; }

        public string SourceType { get; set; } = "SalesOrder";

        public int SourceId { get; set; }
    }
}
