using MediatR;
using TaskManager.Infra.TaskRepository.Read;

namespace TaskManager.Application.Queries.GetTaskByProjectIdQuery;

public class GetTaskByProjectIdQueryHandler(ITaskRepository taskRepository) : IRequestHandler<GetTaskByProjectIdQueryInput, GetTaskByProjectIdQueryResult>
{
    private readonly ITaskRepository _taskRepository = taskRepository ?? throw new ArgumentNullException(nameof(taskRepository));

    public async Task<GetTaskByProjectIdQueryResult> Handle(GetTaskByProjectIdQueryInput request, CancellationToken cancellationToken)
    {
        var tasks = await _taskRepository.GetAsync(g => g.ProjectId == request.ProjectId, cancellationToken);

        return new GetTaskByProjectIdQueryResult
        {
            Items = tasks.Select(s => new Domain.ProjectAggregate.Read.Task
            {
                Id = s.Id,
                ProjectId = s.ProjectId,
                Title = s.Title,
                Description = s.Description,
                Status = s.Status,
                EndDate = s.EndDate,
                Priority = s.Priority,
            }),
        };
    }
}