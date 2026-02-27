using InventorySystem.Domain.Entities.Inventory;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace InventorySystem.Infrastructure.Data.Configurations;

public class InventoryCostLayerConfiguration : BaseEntityConfiguration<InventoryCostLayer>
{
    public override void Configure(EntityTypeBuilder<InventoryCostLayer> builder)
    {
        base.Configure(builder);

        builder.ToTable("InventoryCostLayers");

        builder.Property(x => x.RemainingQty)
            .HasPrecision(18, 4);

        builder.Property(x => x.UnitCost)
            .HasPrecision(18, 4);
    }
}

