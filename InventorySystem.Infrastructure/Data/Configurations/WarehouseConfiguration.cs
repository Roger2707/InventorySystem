using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using InventorySystem.Domain.Entities;

namespace InventorySystem.Infrastructure.Data.Configurations;

public class WarehouseConfiguration : BaseEntityConfiguration<Warehouse>
{
    public override void Configure(EntityTypeBuilder<Warehouse> builder)
    {
        base.Configure(builder);

        // Table name
        builder.ToTable("Warehouses");

        // WarehouseCode - Required, MaxLength 20, Unique
        builder.Property(w => w.WarehouseCode)
            .IsRequired()
            .HasMaxLength(20);

        builder.HasIndex(w => w.WarehouseCode)
            .IsUnique();

        // WarehouseName - Required, MaxLength 100
        builder.Property(w => w.WarehouseName)
            .IsRequired()
            .HasMaxLength(100);

        // Address - Optional, MaxLength 255
        builder.Property(w => w.Address)
            .HasMaxLength(255);

        // PhoneNumber - Optional, MaxLength 20
        builder.Property(w => w.PhoneNumber)
            .HasMaxLength(20);

        // ManagerId - Optional
        builder.Property(w => w.ManagerId)
            .IsRequired(false);

        // IsActive - Default true
        builder.Property(w => w.IsActive)
            .IsRequired()
            .HasDefaultValue(true);

        // Description - Optional, MaxLength 500
        builder.Property(w => w.Description)
            .HasMaxLength(500);

        // CreatedAt - Required
        builder.Property(w => w.CreatedAt)
            .IsRequired();

        // UpdatedAt - Optional
        builder.Property(w => w.UpdatedAt)
            .IsRequired(false);

        // IsDeleted - Required, Default false
        builder.Property(w => w.IsDeleted)
            .IsRequired()
            .HasDefaultValue(false);
    }
}

