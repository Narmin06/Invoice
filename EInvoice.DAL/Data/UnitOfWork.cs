using EInvoice.DAL.Repositories.Implements;
using EInvoice.DAL.Repositories.Interfaces;
using EInvoice.Domain.Models.Common;
using System.Collections.Concurrent;
namespace EInvoice.DAL.Data;

public class UnitOfWork : IUnitOfWork
{
    private readonly AppDbContext _context;
    private readonly ConcurrentDictionary<Type, object> _repositories = new();

    public UnitOfWork(AppDbContext context)
    {
        _context = context;
    }

    public IRepository<TEntity> Repository<TEntity>() where TEntity : BaseEntity
    {
        return (IRepository<TEntity>)_repositories.GetOrAdd(typeof(TEntity)
                , _ => new Repository<TEntity>(_context)
                );
    }

    public Task<int> SaveChangesAsync(CancellationToken cancellationToken = default)
        => _context.SaveChangesAsync(cancellationToken);

    public void Dispose()
        => _context.Dispose();
}