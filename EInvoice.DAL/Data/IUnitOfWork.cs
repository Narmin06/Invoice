using EInvoice.DAL.Repositories.Interfaces;
using EInvoice.Domain.Models.Common;
namespace EInvoice.DAL.Data;

public interface IUnitOfWork : IDisposable
{
    IRepository<TEntity> Repository<TEntity>() where TEntity : BaseEntity;

    Task<int> SaveChangesAsync(CancellationToken cancellationToken = default);
}