using EInvoice.DAL.Data;
using EInvoice.DAL.Repositories.Interfaces;
using EInvoice.Domain.Models;
namespace EInvoice.DAL.Repositories.Implements;

public class InvoiceRepository : Repository<Invoice>, IInvoiceRepository
{
    public InvoiceRepository(AppDbContext context) : base(context)
    {
    }
}