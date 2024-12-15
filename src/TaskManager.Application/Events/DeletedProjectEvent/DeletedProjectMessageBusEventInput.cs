using TaskManager.Core.MessageBus;

namespace TaskManager.Application.Events.DeletedProjectEvent;

public class DeletedProjectMessageBusEventInput(Guid id) : IMessageBusEventInput
{
    public Guid Id { get; } = id;
}
