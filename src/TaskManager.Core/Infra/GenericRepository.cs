using Microsoft.EntityFrameworkCore;
using System.Linq.Expressions;
using TaskManager.Core.Data;
using TaskManager.Core.DomainObjects;

namespace TaskManager.Core.Infra;

public abstract class GenericRepository<TEntity>(ContextBase context) : IGenericRepository<TEntity>, IRepository where TEntity : Entity
{
    private readonly ContextBase _context = context ?? throw new ArgumentNullException("context");

    public IUnitOfWork UnitOfWork => _context;

    public async Task<TEntity?> GetAsync(Expression<Func<TEntity, bool>>? predicate = null, CancellationToken cancellationToken = default)
    {
        var query = GetQuery(predicate);
        return await QueryFirstOrDefaultAsync(query, cancellationToken);
    }

    public async Task<TEntity?> GetAsNoTrackingAsync(Expression<Func<TEntity, bool>>? predicate = null, CancellationToken cancellationToken = default)
    {
        var query = GetQuery(predicate).AsNoTracking();
        return await QueryFirstOrDefaultAsync(query, cancellationToken);
    }

    public async Task AddAsync(TEntity entity, CancellationToken cancellationToken)
        => await _context.AddAsync(entity, cancellationToken);

    public void Update(TEntity entity)
        => _context.Entry(entity).State = EntityState.Modified;

    public void Remove(Guid id)
        => _context.Remove(id);

    public void Remove(TEntity entity)
        => _context.Remove(entity);

    private IQueryable<TEntity> GetQuery(Expression<Func<TEntity, bool>>? predicate)
    {
        var query = _context
                    .Set<TEntity>()
                    .AsQueryable();

        if (predicate != null)
            query = query.Where(predicate);

        return query;
    }

    private static async Task<TEntity?> QueryFirstOrDefaultAsync(IQueryable<TEntity> query, CancellationToken cancellationToken)
        => await query.FirstOrDefaultAsync(cancellationToken);
}