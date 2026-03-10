using EInvoice.DAL.Data;
using EInvoice.DAL.Repositories.Interfaces;
using EInvoice.Domain.Models;
namespace EInvoice.DAL.Repositories.Implements;

public class InvoiceFieldValueRepository : Repository<InvoiceFieldValue>, IInvoiceFieldValueRepository
{
    public InvoiceFieldValueRepository(AppDbContext context) : base(context)
    {
    }
}