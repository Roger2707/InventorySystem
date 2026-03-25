using SharedKernel;

namespace ECommerce.Domain.Entities.Baskets
{
    public class Basket : BaseEntity
    {
        public int CustomerId { get; set; }
        public List<BasketItem> Items { get; set; } = new();
    }
}
