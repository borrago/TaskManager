using MediatR;
using TaskManager.Core.MessageBus;

namespace TaskManager.Application.Events.UpdatedTaskEvent;
public class UpdatedTaskEventHandler(IMessageBus messageBus) : INotificationHandler<UpdatedTaskEventInput>
{
    private readonly IMessageBus _messageBus = messageBus ?? throw new ArgumentNullException(nameof(messageBus));

    public async Task Handle(UpdatedTaskEventInput @event, CancellationToken cancellationToken)
    {
        var brokerEvent = new UpdatedTaskMessageBusEventInput(@event.Id, @event.Title, @event.Description, @event.EndDate, @event.Status);
        await _messageBus.PublishAsync(brokerEvent, cancellationToken);
    }
}
