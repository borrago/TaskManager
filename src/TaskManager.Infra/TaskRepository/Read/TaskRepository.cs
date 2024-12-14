using MongoDB.Driver;
using TaskManager.Core.Infra;

namespace TaskManager.Infra.TaskRepository.Read;

public class TaskRepository(IMongoDatabase database) : GenericReadRepository<Domain.UserAggregate.Read.Task>(database, "Tasks"), ITaskRepository
{
}
