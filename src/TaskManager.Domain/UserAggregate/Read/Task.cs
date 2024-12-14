using TaskManager.Core.DomainObjects;

namespace TaskManager.Domain.UserAggregate.Read;

public class Task : ReadEntity
{
    public string Title { get; private set; } = "";
    public string Description { get; private set; } = "";
    public DateTime EndDate { get; private set; }
    public string Status { get; private set; } = "";
    public string Priority { get; private set; } = "";
    public Guid ProjectId { get; private set; }
    public Project Project { get; private set; } = null!;
}
