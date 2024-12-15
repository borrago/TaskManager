namespace TaskManager.Application.Queries.GetTaskByProjectIdQuery;

public class GetTaskByProjectIdQueryResult
{
    public IEnumerable<Domain.ProjectAggregate.Read.Task> Items { get; set; } = null!;
}
