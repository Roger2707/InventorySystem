using InventorySystem.Domain.Entities.Invoice;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace InventorySystem.Infrastructure.Data.Configurations;

public class InvoiceConfiguration : BaseEntityConfiguration<Invoice>
{
    public override void Configure(EntityTypeBuilder<Invoice> builder)
    {
        base.Configure(builder);

        builder.ToTable("Invoices");

        builder.Property(x => x.TotalAmount)
            .HasPrecision(18, 4);

        builder.Property(x => x.InvoiceDate)
            .IsRequired();
    }
}

