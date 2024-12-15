using TaskManager.Core.MessageBus;

namespace TaskManager.Application.Events.AddedProjectEvent;

public class AddedProjectMessageBusEventInput(Guid id, string name, string description, Guid userId) : IMessageBusEventInput
{
    public Guid Id { get; } = id;
    public string Name { get; } = name;
    public string Description { get; } = description;
    public Guid UserId { get; private set; } = userId;
}
