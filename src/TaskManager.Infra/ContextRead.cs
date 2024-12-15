using MongoDB.Driver;
using TaskManager.Domain.ProjectAggregate.Read;

namespace TaskManager.Infra;

public class ContextRead(IMongoDatabase database)
{
    private readonly IMongoDatabase _database = database;

    public IMongoCollection<Project> Projects => _database.GetCollection<Project>("Projects");
    public IMongoCollection<Domain.ProjectAggregate.Read.Task> Tasks => _database.GetCollection<Domain.ProjectAggregate.Read.Task>("Tasks");
}