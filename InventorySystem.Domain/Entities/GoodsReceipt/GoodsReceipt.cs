using InventorySystem.Domain.Enums;

namespace InventorySystem.Domain.Entities.GoodsReceipt
{
    public class GoodsReceipt : BaseEntity
    {
        public string ReceiptNumber { get; set; }

        public int PurchaseOrderId { get; set; }

        public int WarehouseId { get; set; }

        public ReceiptStatus Status { get; set; }

        public DateTime ReceiptDate { get; set; }
        //public decimal TotalAmount { get; set; }

        public ICollection<GoodsReceiptLine> Lines { get; set; }

        public void Update(int purchaseOrderId, int warehouseId, DateTime receiptDate, List<(int purchaseOrderId, int productId, decimal receivedQty, decimal unitCost)> updatedLines)
        {
            if (Status != ReceiptStatus.Draft)
                throw new Exception("Only Draft Status can be updated !");

            if (updatedLines == null || !updatedLines.Any())
                throw new Exception("There is not any details for this header !");

            PurchaseOrderId = purchaseOrderId;
            WarehouseId = warehouseId;
            ReceiptDate = receiptDate;

            var linesToRemove = Lines.Where(l => !updatedLines.Any(ul => ul.purchaseOrderId == l.PurchaseOrderId && ul.productId == l.ProductId)).ToList();
            foreach (var line in linesToRemove)
                Lines.Remove(line);

            foreach (var updateLine in updatedLines)
            {
                var existLine = Lines.FirstOrDefault(l => l.PurchaseOrderId == updateLine.purchaseOrderId && l.ProductId == updateLine.productId);
                if (existLine != null)
                {
                    existLine.PurchaseOrderId = updateLine.purchaseOrderId;
                    existLine.ProductId = updateLine.productId;
                    existLine.ReceivedQty = updateLine.receivedQty;
                    existLine.UnitCost = updateLine.unitCost;
                }
                else
                {
                    if (updateLine.receivedQty <= 0)
                        throw new Exception("Received quantity must be > 0");
                    var newLine = new GoodsReceiptLine
                    {
                        PurchaseOrderId = updateLine.purchaseOrderId,
                        ProductId = updateLine.productId,
                        ReceivedQty = updateLine.receivedQty,
                        UnitCost = updateLine.unitCost
                    };
                    Lines.Add(newLine);
                }
            }

            //TotalAmount = Lines.Sum(l => l.LineTotal);
        }
    }
}

