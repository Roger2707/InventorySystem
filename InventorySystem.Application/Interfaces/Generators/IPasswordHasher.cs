namespace InventorySystem.Application.Interfaces.Generators;

public interface IPasswordHasher
{
    string HashPassword(string password);
    bool VerifyPassword(string password, string passwordHash);
}

