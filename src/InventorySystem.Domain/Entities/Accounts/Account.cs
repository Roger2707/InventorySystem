using InventorySystem.Domain.Enums;

namespace InventorySystem.Domain.Entities.Accounts
{
    public class Account : BaseEntity
    {
        public string Code { get; set; }

        public string Name { get; set; }

        public AccountType Type { get; set; }

        public bool IsActive { get; set; } = true;
    }
}