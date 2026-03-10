using EInvoice.DAL.Data;
using EInvoice.DAL.Repositories.Interfaces;
using EInvoice.Domain.Models;
namespace EInvoice.DAL.Repositories.Implements;

public class InvoiceFieldDefinitionRepository : Repository<InvoiceFieldDefinition>, IInvoiceFieldDefinitionRepository
{
    public InvoiceFieldDefinitionRepository(AppDbContext context) : base(context)
    {
    }
}