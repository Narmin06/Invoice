using EInvoice.Domain.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace EInvoice.DAL.Configurations;

public class ExporterConfiguration : IEntityTypeConfiguration<Exporter>
{
    public void Configure(EntityTypeBuilder<Exporter> builder)
    {
        builder.OwnsOne(exp => exp.Address);
    }
}