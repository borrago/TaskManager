using TaskManager.Core.DomainObjects;

namespace TaskManager.Domain.UserAggregate.Read;

public class Project : ReadEntity
{
    public string Name { get; private set; } = "";
    public string Description { get; private set; } = "";
    public Guid UserId { get; private set; }
    public IEnumerable<Task> Tasks { get; private set; } = null!;
}
