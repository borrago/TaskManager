using MongoDB.Driver;
using TaskManager.Core.Infra;

namespace TaskManager.Infra.TaskRepository.Read;

public class TaskRepository(IMongoDatabase database) : GenericReadRepository<Domain.ProjectAggregate.Read.Task>(database, "Tasks"), ITaskRepository
{
}
