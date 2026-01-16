namespace InventorySystem.Application.Interfaces;

public interface IPasswordHasher
{
    /// <summary>
    /// Hashes a password
    /// </summary>
    string HashPassword(string password);

    /// <summary>
    /// Verifies a password against a hash
    /// </summary>
    bool VerifyPassword(string password, string passwordHash);
}

