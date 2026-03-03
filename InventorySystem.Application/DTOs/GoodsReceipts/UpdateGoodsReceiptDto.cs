namespace InventorySystem.Application.DTOs.GoodsReceipts
{
    public class UpdateGoodsReceiptDto
    {
        public int PurchaseOrderId { get; set; }
        public int WarehouseId { get; set; }
        public List<UpdateGoodsReceiptLineDto> LinesDto { get; set; }
    }

    public class UpdateGoodsReceiptLineDto
    {
        public int PurchaseOrderId { get; set; }
        public int ProductId { get; set; }
        public decimal ReceivedQty { get; set; }
    }
}
