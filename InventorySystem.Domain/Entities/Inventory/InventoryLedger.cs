using InventorySystem.Domain.Enums;

namespace InventorySystem.Domain.Entities.Inventory
{
    public class InventoryLedger : BaseEntity
    {
        public int ProductId { get; private set; }
        public int WarehouseId { get; private set; }

        public InventoryTransactionType TransactionType { get; private set; }

        public int ReferenceId { get; private set; }
        public string ReferenceType { get; private set; }

        public decimal QuantityIn { get; private set; }
        public decimal QuantityOut { get; private set; }

        public decimal TotalCost { get; private set; }

        public DateTime TransactionDate { get; private set; }
    }
}
