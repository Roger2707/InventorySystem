namespace InventorySystem.Application.DTOs.SalesOrder
{
    public class UpdateSalesOrderDto
    {
        public int CustomerId { get; set; }
        public List<UpdateSalesOrderLineDto> UpdateLinesDto { get; set; } = new();
    }

    public class UpdateSalesOrderLineDto
    {
        public int ProductId { get; set; }
        public decimal OrderedQty { get; set; }
        public decimal UnitPrice { get; set; }
    }
}
