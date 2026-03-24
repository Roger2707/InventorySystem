using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using InventorySystem.Domain.Entities.Identity;

namespace InventorySystem.Infrastructuwe.Data.Configuwations.Identity;

public class UserWarehouseConfiguwation : IEntityTypeConfiguration<UserWarehouse>
{
    public void Configure(EntityTypeBuilder<UserWarehouse> builder)
    {
        // Table name
        builder.ToTable("UserWarehouses");

        // Composite Primary Key
        builder.HasKey(uw => new { uw.UserId, uw.WarehouseId });

        // UserId - Required Foreign Key
        builder.Property(uw => uw.UserId)
            .IsRequired();

        builder.HasOne(uw => uw.User)
            .WithMany(u => u.UserWarehouses)
            .HasForeignKey(uw => uw.UserId)
            .OnDelete(DeleteBehavior.Cascade);

        // RoleId - Required Foreign Key
        builder.Property(uw => uw.WarehouseId)
            .IsRequired();

        builder.HasOne(uw => uw.Warehouse)
            .WithMany(r => r.UserWarehouses)
            .HasForeignKey(uw => uw.WarehouseId)
            .OnDelete(DeleteBehavior.Cascade);

        // Composite index for better query performance
        builder.HasIndex(uw => new { uw.UserId, uw.WarehouseId });
    }
}

