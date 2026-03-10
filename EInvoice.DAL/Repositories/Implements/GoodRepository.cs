using EInvoice.DAL.Data;
using EInvoice.DAL.Repositories.Interfaces;
using EInvoice.Domain.Models;
namespace EInvoice.DAL.Repositories.Implements;

public class GoodRepository : Repository<Good>, IGoodRepository
{
    public GoodRepository(AppDbContext context) : base(context)
    {
    }
}