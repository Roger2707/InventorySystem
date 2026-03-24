namespace InventorySystem.Application.DTOs.Baskets
{
    public class UpdateBasketDto
    {
        public int CustomerId { get; set; }
        public List<UpdateBasketItemDto> Items { get; set; } = new();
    }

    public class UpdateBasketItemDto
    {
        public int ProductId { get; set; }
        public decimal Quantity { get; set; }
    }
}
