using MediatR;
using TaskManager.Core.MessageBus;

namespace TaskManager.Application.Events.AddedTaskEvent;

public class AddedTaskEventHandler(IMessageBus messageBus) : INotificationHandler<AddedTaskEventInput>
{
    private readonly IMessageBus _messageBus = messageBus ?? throw new ArgumentNullException(nameof(messageBus));

    public async Task Handle(AddedTaskEventInput @event, CancellationToken cancellationToken)
    {
        var brokerEvent = new AddedTaskMessageBusEventInput(@event.Id, @event.ProjectId, @event.Title, @event.Description, @event.EndDate, @event.Status, @event.Priority);
        await _messageBus.PublishAsync(brokerEvent, cancellationToken);
    }
}
