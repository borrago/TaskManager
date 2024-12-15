using TaskManager.Application.Queries.GetTaskByIdQuery;

namespace TaskManager.Application.Queries.GetTaskByProjectIdQuery;

public class GetTaskByProjectIdQueryResult
{
    public IEnumerable<GetTaskByIdQueryResult> Items { get; set; } = null!;
}
