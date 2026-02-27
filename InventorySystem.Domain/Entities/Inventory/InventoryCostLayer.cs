namespace InventorySystem.Domain.Entities.Inventory
{
    public class InventoryCostLayer : BaseEntity
    {
        public int ProductId { get; private set; }
        public int WarehouseId { get; private set; }

        public int SourceReceiptLineId { get; private set; }

        public decimal RemainingQty { get; private set; }

        public decimal UnitCost { get; private set; }

        public bool IsClosed => RemainingQty <= 0;
    }
}
