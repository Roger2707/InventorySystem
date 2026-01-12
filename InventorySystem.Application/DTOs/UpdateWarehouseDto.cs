using System.ComponentModel.DataAnnotations;

namespace InventorySystem.Application.DTOs;

public class UpdateWarehouseDto
{
    [Required]
    [StringLength(20, MinimumLength = 1)]
    public string WarehouseCode { get; set; } = string.Empty;

    [Required]
    [StringLength(100, MinimumLength = 1)]
    public string WarehouseName { get; set; } = string.Empty;

    [StringLength(255)]
    public string? Address { get; set; }

    [StringLength(20)]
    public string? PhoneNumber { get; set; }

    [Range(0, double.MaxValue, ErrorMessage = "Area must be a positive number")]
    public decimal? Area { get; set; }

    public int? ManagerId { get; set; }

    public bool IsActive { get; set; } = true;

    [StringLength(500)]
    public string? Description { get; set; }
}

