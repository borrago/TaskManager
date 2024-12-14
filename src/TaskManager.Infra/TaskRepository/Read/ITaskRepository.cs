using TaskManager.Core.Infra;

namespace TaskManager.Infra.TaskRepository.Read;

public interface ITaskRepository : IGenericReadRepository<Domain.UserAggregate.Read.Task>
{
}