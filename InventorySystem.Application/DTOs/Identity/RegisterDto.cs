namespace InventorySystem.Application.DTOs.Identity;

public class RegisterDto
{
    public required string Username { get; set; }
    public required string Password { get; set; }
    public required string FullName { get; set; }
    public required string Email { get; set; }
    public string? PhoneNumber { get; set; }
    public string? Address { get; set; }
    public int? WarehouseId { get; set; }
}

