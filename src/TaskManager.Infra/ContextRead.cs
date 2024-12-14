using MongoDB.Driver;
using TaskManager.Domain.UserAggregate.Read;

namespace TaskManager.Infra;

public class ContextRead(IMongoDatabase database)
{
    private readonly IMongoDatabase _database = database;

    public IMongoCollection<Project> Projects => _database.GetCollection<Project>("Projects");
    public IMongoCollection<Domain.UserAggregate.Read.Task> Tasks => _database.GetCollection<Domain.UserAggregate.Read.Task>("Tasks");
}