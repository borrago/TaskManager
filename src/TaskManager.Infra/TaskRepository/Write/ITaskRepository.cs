using TaskManager.Core.Infra;

namespace TaskManager.Infra.TaskRepository.Write;

public interface ITaskRepository : IGenericRepository<Domain.ProjectAggregate.Write.Task>
{
}