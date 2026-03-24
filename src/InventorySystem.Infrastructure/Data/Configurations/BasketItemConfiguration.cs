using InventorySystem.Domain.Entities.Baskets;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace InventorySystem.Infrastructure.Data.Configurations
{
    public class BasketItemConfiguration : IEntityTypeConfiguration<BasketItem>
    {
        public void Configure(EntityTypeBuilder<BasketItem> builder)
        {
            builder.ToTable("BasketItems");

            builder.HasKey(x => new { x.BasketId, x.ProductId, x.RowNumber });

            builder.Property(x => x.Quantity)
                .HasPrecision(18, 4);

            builder.Property(x => x.UnitPrice)
                .HasPrecision(18, 4);

            builder.Ignore(x => x.LineTotal);

            builder.HasOne(x => x.Product)
                .WithMany()
                .HasForeignKey(x => x.ProductId)
                .OnDelete(DeleteBehavior.Restrict);

            builder.HasOne<Basket>()
                .WithMany(x => x.Items)
                .HasForeignKey(x => x.BasketId)
                .OnDelete(DeleteBehavior.Cascade);
        }
    }
}
