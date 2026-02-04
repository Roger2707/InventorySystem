using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using InventorySystem.Domain.Entities.Identity;

namespace InventorySystem.Infrastructure.Data.Configurations.Identity;

public class UserRegionConfiguration : IEntityTypeConfiguration<UserRegion>
{
    public void Configure(EntityTypeBuilder<UserRegion> builder)
    {
        // Table name
        builder.ToTable("UserRegions");

        // Composite Primary Key
        builder.HasKey(ur => new { ur.UserId, ur.RegionId });

        // UserId - Required Foreign Key
        builder.Property(ur => ur.UserId)
            .IsRequired();

        builder.HasOne(ur => ur.User)
            .WithMany(u => u.UserRegions)
            .HasForeignKey(ur => ur.UserId)
            .OnDelete(DeleteBehavior.Cascade);

        // RoleId - Required Foreign Key
        builder.Property(ur => ur.RegionId)
            .IsRequired();

        builder.HasOne(ur => ur.Region)
            .WithMany(r => r.UserRegions)
            .HasForeignKey(ur => ur.RegionId)
            .OnDelete(DeleteBehavior.Cascade);

        // Composite index for better query performance
        builder.HasIndex(ur => new { ur.UserId, ur.RegionId });
    }
}

