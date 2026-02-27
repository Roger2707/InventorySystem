using InventorySystem.Domain.Enums;

namespace InventorySystem.Application.DTOs.PurchaseOrders;

public class PurchaseOrderDto
{
    public int Id { get; set; }
    public string OrderNumber { get; set; } = string.Empty;
    public int SupplierId { get; set; }
    public PurchaseOrderStatus Status { get; set; }
    public DateTime OrderDate { get; set; }
    public DateTime? ApprovedDate { get; set; }
    public decimal TotalAmount { get; set; }

    public List<PurchaseOrderLineDto> Lines { get; set; } = new();
}

