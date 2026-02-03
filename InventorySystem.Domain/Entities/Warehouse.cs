namespace InventorySystem.Domain.Entities;

public class Warehouse : BaseEntity
{
    public required string WarehouseCode { get; set; }

    public required string WarehouseName { get; set; }

    public string? Address { get; set; }
    public WarehouseRegion Region { get; set; } = WarehouseRegion.South;

    public string? PhoneNumber { get; set; }

    public int? ManagerId { get; set; }

    public bool IsActive { get; set; } = true;

    public string? Description { get; set; }
}

public enum WarehouseRegion
{
    South = 1,
    North = 2,
    West = 3,
}


