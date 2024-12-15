using TaskManager.Core.MessageBus;

namespace TaskManager.Application.Events.DeleteProjectEvent;

public class DeletedProjectEventInput(Guid id) : IEventInput
{
    public Guid Id { get; } = id;
}
