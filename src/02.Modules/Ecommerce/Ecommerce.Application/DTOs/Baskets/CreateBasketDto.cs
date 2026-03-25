namespace ECommerce.Application.DTOs.Baskets
{
    public class CreateBasketDto
    {
        public int CustomerId { get; set; }
        public List<CreateBasketItemDto> Items { get; set; } = new();
    }

    public class CreateBasketItemDto
    {
        public int ProductId { get; set; }
        public decimal Quantity { get; set; }
        public decimal UnitPrice { get; set; }
    }
}
