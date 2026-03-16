using EInvoice.Domain.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
namespace EInvoice.DAL.Configurations;

public class InvoiceConfiguration : IEntityTypeConfiguration<Invoice>
{
    public void Configure(EntityTypeBuilder<Invoice> builder)
    {
        builder.OwnsOne(i => i.InvoiceRequisites);
        builder.OwnsOne(i => i.CircumstancesAffectingInvoice);
    }
}