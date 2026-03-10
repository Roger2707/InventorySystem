using InventorySystem.Domain.Entities.SalesOrder;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace InventorySystem.Infrastructure.Data.Configurations;

public class SalesOrderLineConfiguration : IEntityTypeConfiguration<SalesOrderLine>
{
    public void Configure(EntityTypeBuilder<SalesOrderLine> builder)
    {
        builder.ToTable("SalesOrderLines");

        builder.HasKey(x => new { x.SalesOrderId, x.ProductId });

        builder.Property(x => x.OrderedQty)
            .HasPrecision(18, 4);

        builder.Property(x => x.DeliveredQty)
            .HasPrecision(18, 4);

        builder.Property(x => x.UnitPrice)
            .HasPrecision(18, 4);

        builder.Ignore(x => x.RemainingQty);
        builder.Ignore(x => x.LineTotal);

        builder.HasOne(x => x.Product)
            .WithMany()
            .HasForeignKey(x => x.ProductId)
            .OnDelete(DeleteBehavior.Restrict);

        builder.HasOne<SalesOrder>()
            .WithMany(x => x.Lines)
            .HasForeignKey(x => x.SalesOrderId)
            .OnDelete(DeleteBehavior.Restrict);
    }
}

