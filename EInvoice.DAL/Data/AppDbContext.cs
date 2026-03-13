using EInvoice.Domain.Models;
using Microsoft.EntityFrameworkCore;
namespace EInvoice.DAL.Data;

public class AppDbContext : DbContext
{
    public AppDbContext(DbContextOptions<AppDbContext> options) : base(options) { }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.ApplyConfigurationsFromAssembly(typeof(AppDbContext).Assembly);

        base.OnModelCreating(modelBuilder); 
    }

    public DbSet<Good> Goods => Set<Good>();
    public DbSet<Invoice> Invoices => Set<Invoice>();
    public DbSet<Importer> Importers => Set<Importer>();
    public DbSet<Exporter> Exporters => Set<Exporter>();
    public DbSet<InvoiceFieldValue> InvoiceFieldValues => Set<InvoiceFieldValue>();
    public DbSet<InvoiceFieldDefinition> InvoiceFieldDefinitions => Set<InvoiceFieldDefinition>();
}