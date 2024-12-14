using TaskManager.Core.DomainObjects;

namespace TaskManager.Domain.UserAggregate.Read;

public class Project : ReadEntity
{
    public string Name { get; private set; } = "";
    public Guid UserId { get; private set; }
    public User User { get; private set; } = null!;
    public IEnumerable<Task> Tasks { get; private set; } = null!;
}
