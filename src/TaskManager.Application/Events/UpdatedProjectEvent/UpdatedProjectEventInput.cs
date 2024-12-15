using TaskManager.Core.MessageBus;

namespace TaskManager.Application.Events.UpdatedProjectEvent;

public class UpdatedProjectEventInput(Guid id, string name, string description) : IEventInput
{
    public Guid Id { get; } = id;
    public string Name { get; } = name;
    public string Description { get; } = description;
}
