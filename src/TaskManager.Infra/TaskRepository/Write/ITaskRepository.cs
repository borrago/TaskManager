using TaskManager.Core.Infra;

namespace TaskManager.Infra.TaskRepository.Write;

public interface ITaskRepository : IGenericRepository<Domain.UserAggregate.Write.Task>
{
}