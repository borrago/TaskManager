using MediatR;
using TaskManager.Infra.TaskRepository.Read;

namespace TaskManager.Application.Queries.GetTaskByIdQuery;

public class GetTaskByIdQueryHandler(ITaskRepository taskRepository) : IRequestHandler<GetTaskByIdQueryInput, GetTaskByIdQueryResult>
{
    private readonly ITaskRepository _taskRepository = taskRepository ?? throw new ArgumentNullException(nameof(taskRepository));

    public async Task<GetTaskByIdQueryResult> Handle(GetTaskByIdQueryInput request, CancellationToken cancellationToken)
    {
        var task = await _taskRepository.GetByIdAsync(request.Id, cancellationToken) ?? throw new ApplicationException("Tarefa não localizada.");

        return new GetTaskByIdQueryResult
        {
            Id = task.Id,
            Title = task.Title,
            Description = task.Description,
            EndDate = task.EndDate,
            Priority = task.Priority,
            ProjectId = task.ProjectId,
            Status = task.Status,
        };
    }
}