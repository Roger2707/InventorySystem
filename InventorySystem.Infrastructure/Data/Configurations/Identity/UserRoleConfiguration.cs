using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using InventorySystem.Domain.Entities.Identity;

namespace InventorySystem.Infrastructure.Data.Configurations.Identity;

public class UserRoleConfiguration : IEntityTypeConfiguration<UserRole>
{
    public void Configure(EntityTypeBuilder<UserRole> builder)
    {
        // Table name
        builder.ToTable("UserRoles");

        // Composite Primary Key
        builder.HasKey(ur => new { ur.UserId, ur.RoleId });

        // UserId - Required Foreign Key
        builder.Property(ur => ur.UserId)
            .IsRequired();

        builder.HasOne(ur => ur.User)
            .WithMany(u => u.UserRoles)
            .HasForeignKey(ur => ur.UserId)
            .OnDelete(DeleteBehavior.Cascade);

        // RoleId - Required Foreign Key
        builder.Property(ur => ur.RoleId)
            .IsRequired();

        builder.HasOne(ur => ur.Role)
            .WithMany(r => r.UserRoles)
            .HasForeignKey(ur => ur.RoleId)
            .OnDelete(DeleteBehavior.Cascade);

        // AssignedAt - Required, Default DateTime.Now
        builder.Property(ur => ur.AssignedAt)
            .IsRequired()
            .HasDefaultValueSql("GETUTCDATE()");

        // Composite index for better query performance
        builder.HasIndex(ur => new { ur.UserId, ur.RoleId });
    }
}

