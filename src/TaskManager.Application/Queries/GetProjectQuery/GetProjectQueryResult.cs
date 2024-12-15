namespace TaskManager.Application.Queries.GetProjectQuery;

public class GetProjectQueryResult
{
    public IEnumerable<ProjectResult> Items { get; set; } = null!;
}

public class ProjectResult
{
    public Guid Id { get; set; }
    public string Name { get; set; } = "";
    public string Description { get; set; } = "";
    public Guid UserId { get; set; }
}