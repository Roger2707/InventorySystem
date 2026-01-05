using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace InventorySystem.Domain.Entities;

public class Warehouse : BaseEntity
{
    [Required]
    [MaxLength(20)]
    public string WarehouseCode { get; set; }

    [Required]
    [MaxLength(100)]
    public string WarehouseName { get; set; }

    [MaxLength(255)]
    public string? Address { get; set; }

    [MaxLength(20)]
    public string? PhoneNumber { get; set; }

    [Column(TypeName = "decimal(18,2)")]
    public decimal? Area { get; set; } // m2

    public int? ManagerId { get; set; }

    public bool IsActive { get; set; } = true;

    [MaxLength(500)]
    public string? Description { get; set; }
}


