namespace InventorySystem.Application.DTOs.PurchaseOrders
{
    public class UpdatePurchaseOrderDto
    {
        public int SupplierId { get; set; }
        public DateTime OrderDate { get; set; }

        public List<UpdatePurchaseOrderLineDto> LinesDto { get; set; } = new();
    }

    public class UpdatePurchaseOrderLineDto
    {
        public int ProductId { get; set; }
        public decimal OrderedQty { get; set; }
    }
}
