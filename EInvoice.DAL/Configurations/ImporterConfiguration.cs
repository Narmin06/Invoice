using EInvoice.Domain.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

public class ImporterConfiguration : IEntityTypeConfiguration<Importer>
{
    public void Configure(EntityTypeBuilder<Importer> builder)
    {
        builder.OwnsOne(imp => imp.Address);
    }
}