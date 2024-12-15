using TaskManager.Domain.ProjectAggregate.Read;

namespace TaskManager.Application.Queries.GetProjectQuery;

public class GetProjectQueryResult
{
    public IEnumerable<Project> Items { get; set; } = null!;
}
