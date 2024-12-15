using TaskManager.Core.MessageBus;

namespace TaskManager.Application.Events.UpdatedProjectEvent;

public class UpdatedProjectMessageBusEventInput(Guid id, string name, string description) : IMessageBusEventInput
{
    public Guid Id { get; } = id;
    public string Name { get; } = name;
    public string Description { get; } = description;
}
