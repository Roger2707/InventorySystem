using InventorySystem.Domain.Entities.Baskets;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace InventorySystem.Infrastructure.Data.Configurations
{
    public class BasketConfiguration : BaseEntityConfiguration<Basket>
    {
        public override void Configure(EntityTypeBuilder<Basket> builder)
        {
            base.Configure(builder);

            builder.ToTable("Baskets");

            builder.HasOne(x => x.Customer)
                .WithMany()
                .HasForeignKey(x => x.CustomerId)
                .OnDelete(Microsoft.EntityFrameworkCore.DeleteBehavior.Restrict);
        }
    }
}
