﻿using MongoDB.Driver;
using System.Linq.Expressions;
using TaskManager.Core.DomainObjects;

namespace TaskManager.Core.Infra;

public interface IGenericReadRepository<T> where T : IReadEntity
{
    Task<bool> InsertAsync(T entity, CancellationToken cancellationToken);
    Task<bool> UpdateAsync(T entity, CancellationToken cancellationToken);
    Task<bool> RemoveAsync(Guid id, CancellationToken cancellationToken);
    Task<List<T>> GetAllAsync(CancellationToken cancellationToken);
    Task<T> GetByIdAsync(Guid id, CancellationToken cancellationToken);
    Task<List<T>> GetAsync(Expression<Func<T, bool>>? predicate = null, CancellationToken cancellationToken = default);
    Task<bool> RemoveManyAsync(FilterDefinition<T> filter, CancellationToken cancellationToken);
}