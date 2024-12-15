using TaskManager.Application.Queries.GetProjectQuery;
using TaskManager.Application.Queries.GetTaskByIdQuery;

namespace TaskManager.Application.Queries.GetProjectByIdQuery;

public class GetProjectByIdQueryResult : ProjectResult
{
    public IEnumerable<GetTaskByIdQueryResult> Tasks { get; set; } = null!;
}
