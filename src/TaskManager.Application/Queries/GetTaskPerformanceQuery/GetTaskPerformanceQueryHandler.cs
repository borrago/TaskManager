using MediatR;
using TaskManager.Domain.ProjectAggregate.Write;
using TaskManager.Infra.TaskRepository.Read;

namespace TaskManager.Application.Queries.GetTaskPerformanceQuery;

public class GetTaskPerformanceQueryHandler(ITaskRepository taskRepository) : IRequestHandler<GetTaskPerformanceQueryInput, GetTaskPerformanceQueryResult>
{
    private readonly ITaskRepository _taskRepository = taskRepository ?? throw new ArgumentNullException(nameof(taskRepository));

    public async Task<GetTaskPerformanceQueryResult> Handle(GetTaskPerformanceQueryInput request, CancellationToken cancellationToken)
    {
        var startDate = DateTime.UtcNow.AddDays(-30);

        var completedTasks = await _taskRepository.GetAsync(
            t => t.Status == TaskStatusEnum.Concluida.ToString() && t.EndDate >= startDate,
            cancellationToken
        );

        var report = completedTasks
            .GroupBy(t => t.AssignedUserId)
            .Select(g => new Performance
            {
                UserId = g.Key,
                CompletedTasks = g.Count(),
                AverageTasks = g.Count() / 30
            })
            .OrderByDescending(r => r.CompletedTasks)
            .ToList();

        return new GetTaskPerformanceQueryResult
        {
            Items = report,
        };
    }
}