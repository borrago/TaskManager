using TaskManager.Core.MessageBus;

namespace TaskManager.Application.Events.DeleteProjectEvent;

public class DeletedProjectMessageBusEventInput(Guid id) : IMessageBusEventInput
{
    public Guid Id { get; } = id;
}
