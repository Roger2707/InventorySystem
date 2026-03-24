using InventorySystem.Domain.Entities.Suppliers;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace InventorySystem.Infrastructure.Data.Configurations;

public class SupplierProductPriceConfiguration : BaseEntityConfiguration<SupplierProductPrice>
{
    public override void Configure(EntityTypeBuilder<SupplierProductPrice> builder)
    {
        base.Configure(builder);

        builder.ToTable("SupplierProductPrices");

        builder.Property(x => x.UnitPrice)
            .HasPrecision(18, 4);
    }
}

