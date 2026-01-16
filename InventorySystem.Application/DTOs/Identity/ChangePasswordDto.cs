namespace InventorySystem.Application.DTOs.Identity;

public class ChangePasswordDto
{
    public required string CurrentPassword { get; set; }
    public required string NewPassword { get; set; }
}

