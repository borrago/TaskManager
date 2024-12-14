using TaskManager.Core.DomainObjects;

namespace TaskManager.Domain.UserAggregate.Read;

public class User : ReadEntity
{
    public string Name { get; private set; } = "";
    public string Email { get; private set; } = "";
}
