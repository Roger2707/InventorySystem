using InventorySystem.Domain.Entities.Inventory;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace InventorySystem.Infrastructure.Data.Configurations;

public class InventoryReservationConfiguration : BaseEntityConfiguration<InventoryReservation>
{
    public override void Configure(EntityTypeBuilder<InventoryReservation> builder)
    {
        base.Configure(builder);

        builder.ToTable("InventoryReservations");

        builder.Property(x => x.ReservedQty)
            .HasPrecision(18, 4);

        builder.Property(x => x.UnitPrice)
            .HasPrecision(18, 4);

        builder.Ignore(x => x.TotalAmount);
    }
}

