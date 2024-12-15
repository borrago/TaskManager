using TaskManager.Core.MessageBus;

namespace TaskManager.Application.Events.DeletedTaskEvent;

public class DeletedTaskMessageBusEventInput(Guid id) : IMessageBusEventInput
{
    public Guid Id { get; } = id;
}
