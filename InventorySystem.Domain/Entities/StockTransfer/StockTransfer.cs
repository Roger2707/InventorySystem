using InventorySystem.Domain.Enums;

namespace InventorySystem.Domain.Entities.StockTransfer
{
    public class StockTransfer : BaseEntity
    {
        public string TransferNumber { get; private set; }

        public int FromWarehouseId { get; private set; }
        public int ToWarehouseId { get; private set; }

        public TransferStatus Status { get; private set; }

        public ICollection<StockTransferLine> Lines { get; private set; }
    }
}
