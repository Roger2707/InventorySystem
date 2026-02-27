using InventorySystem.Domain.Entities.StockTransfer;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace InventorySystem.Infrastructure.Data.Configurations;

public class StockTransferConfiguration : BaseEntityConfiguration<StockTransfer>
{
    public override void Configure(EntityTypeBuilder<StockTransfer> builder)
    {
        base.Configure(builder);

        builder.ToTable("StockTransfers");

        builder.Property(x => x.TransferNumber)
            .IsRequired()
            .HasMaxLength(50);
    }
}

