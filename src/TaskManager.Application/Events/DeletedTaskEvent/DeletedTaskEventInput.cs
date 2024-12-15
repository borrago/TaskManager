using TaskManager.Core.MessageBus;

namespace TaskManager.Application.Events.DeletedTaskEvent;

public class DeletedTaskEventInput(Guid id) : IEventInput
{
    public Guid Id { get; } = id;
}
