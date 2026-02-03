namespace InventorySystem.Domain.Entities.Identity
{
    public class UserWarehouse
    {
        public int UserId { get; set; }
        public User User { get; set; }

        public int WarehouseId { get; set; }
        public Warehouse Warehouse { get; set; }
    }
}
