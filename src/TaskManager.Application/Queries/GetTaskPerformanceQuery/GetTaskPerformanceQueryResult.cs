namespace TaskManager.Application.Queries.GetTaskPerformanceQuery;

public class GetTaskPerformanceQueryResult
{
    public IEnumerable<Performance> Items { get; set; } = null!;
}

public class Performance
{
    public Guid UserId { get; set; }
    public int CompletedTasks { get; set; }
    public double AverageTasks { get; set; }
}