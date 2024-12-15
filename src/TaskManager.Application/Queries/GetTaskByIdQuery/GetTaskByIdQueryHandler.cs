using MediatR;
using TaskManager.Infra.TaskRepository.Read;

namespace TaskManager.Application.Queries.GetTaskByIdQuery;

internal class GetTaskByIdQueryHandler(ITaskRepository taskRepository) : IRequestHandler<GetTaskByIdQueryInput, GetTaskByIdQueryResult>
{
    private readonly ITaskRepository _taskRepository = taskRepository ?? throw new ArgumentNullException(nameof(taskRepository));

    public async Task<GetTaskByIdQueryResult> Handle(GetTaskByIdQueryInput request, CancellationToken cancellationToken)
    {
        var client = await _taskRepository.GetByIdAsync(request.Id, cancellationToken) ?? throw new Exception("Tarefa não localizado.");

        return new GetTaskByIdQueryResult
        {
            Id = client.Id,
            Title = client.Title,
            Description = client.Description,
            EndDate = client.EndDate,
            Priority = client.Priority,
            ProjectId = client.ProjectId,
            Status = client.Status,
        };
    }
}