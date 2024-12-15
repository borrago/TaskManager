using TaskManager.Core.Infra;

namespace TaskManager.Infra.TaskRepository.Write;

public class TaskRepository(Context context) : GenericRepository<Domain.ProjectAggregate.Write.Task>(context), ITaskRepository
{
}