using InventorySystem.Domain.Entities.GoodsReceipt;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace InventorySystem.Infrastructure.Data.Configurations;

public class GoodsReceiptLineConfiguration : BaseEntityConfiguration<GoodsReceiptLine>
{
    public override void Configure(EntityTypeBuilder<GoodsReceiptLine> builder)
    {
        base.Configure(builder);

        builder.ToTable("GoodsReceiptLines");

        builder.Property(x => x.ReceivedQty)
            .HasPrecision(18, 4);

        builder.Property(x => x.UnitCost)
            .HasPrecision(18, 4);

        builder.Ignore(x => x.LineTotal);

        builder.HasOne<GoodsReceipt>()
            .WithMany(x => x.Lines)
            .HasForeignKey(x => x.GoodsReceiptId)
            .OnDelete(DeleteBehavior.Restrict);
    }
}

