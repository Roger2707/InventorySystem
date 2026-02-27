using InventorySystem.Domain.Entities.Inventory;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace InventorySystem.Infrastructure.Data.Configurations;

public class InventoryLedgerConfiguration : BaseEntityConfiguration<InventoryLedger>
{
    public override void Configure(EntityTypeBuilder<InventoryLedger> builder)
    {
        base.Configure(builder);

        builder.ToTable("InventoryLedgers");

        builder.Property(x => x.QuantityIn)
            .HasPrecision(18, 4);

        builder.Property(x => x.QuantityOut)
            .HasPrecision(18, 4);

        builder.Property(x => x.TotalCost)
            .HasPrecision(18, 4);
    }
}

