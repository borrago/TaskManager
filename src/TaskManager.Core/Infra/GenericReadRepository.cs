using MongoDB.Driver;
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
        var result = await _collection.ReplaceOneAsync(c => c.Id == entity.Id, entity, new ReplaceOptions(), cancellationToken);
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
}