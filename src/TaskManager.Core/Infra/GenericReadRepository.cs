using MongoDB.Driver;
using System.Linq.Expressions;
using TaskManager.Core.DomainObjects;

namespace TaskManager.Core.Infra;

public class GenericReadRepository<T>(IMongoDatabase database, string collectionName) : IGenericReadRepository<T> where T : IReadEntity
{
    private readonly IMongoCollection<T> _collection = database.GetCollection<T>(collectionName);

    public async Task<bool> InsertAsync(T entity, CancellationToken cancellationToken)
    {
        await _collection.InsertOneAsync(entity, new InsertOneOptions(), cancellationToken);
        return true;
    }

    public async Task<bool> UpdateAsync(T entity, CancellationToken cancellationToken)
    {
        var updateDefinitions = new List<UpdateDefinition<T>>();
        var properties = typeof(T).GetProperties();
        
        foreach (var property in properties)
        {
            var value = property.GetValue(entity);
            if (value != null && !IsDefaultValue(value))
            {
                updateDefinitions.Add(Builders<T>.Update.Set(property.Name, value));
            }
        }

        var updateDefinition = Builders<T>.Update.Combine(updateDefinitions);
        var filter = Builders<T>.Filter.Eq(p => p.Id, entity.Id);
        var result = await _collection.UpdateOneAsync(filter, updateDefinition, new UpdateOptions(), cancellationToken);

        return result.IsAcknowledged && result.ModifiedCount > 0;
    }

    public async Task<bool> RemoveAsync(Guid id, CancellationToken cancellationToken)
    {
        var result = await _collection.DeleteOneAsync(c => c.Id == id, cancellationToken);
        return result.IsAcknowledged && result.DeletedCount > 0;
    }

    public Task<List<T>> GetAllAsync(CancellationToken cancellationToken)
        => _collection.Find(_ => true).ToListAsync(cancellationToken);

    public Task<T> GetByIdAsync(Guid id, CancellationToken cancellationToken)
        => _collection.Find(entity => entity.Id == id).FirstOrDefaultAsync(cancellationToken);

    public Task<List<T>> GetAsync(Expression<Func<T, bool>>? predicate = null, CancellationToken cancellationToken = default)
    {
        var filter = predicate ?? (_ => true);
        return _collection.Find(filter).ToListAsync(cancellationToken);
    }

    private static bool IsDefaultValue(object value)
    {
        var type = value.GetType();
        return value.Equals(type.IsValueType ? Activator.CreateInstance(type) : null);
    }
}