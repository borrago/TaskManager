using System.Linq.Expressions;
using TaskManager.Core.Data;
using TaskManager.Core.DomainObjects;

namespace TaskManager.Core.Infra;

public interface IGenericRepository<TEntity> : IRepository where TEntity : IEntity
{
    Task<TEntity?> GetAsync(Expression<Func<TEntity, bool>>? predicate = null, CancellationToken cancellationToken = default);
    Task<TEntity?> GetAsNoTrackingAsync(Expression<Func<TEntity, bool>>? predicate = null, CancellationToken cancellationToken = default);
    Task AddAsync(TEntity entity, CancellationToken cancellationToken);
    void Update(TEntity entity);
    void Remove(Guid id);
    void Remove(TEntity entity);
}