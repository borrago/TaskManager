using MediatR;
using TaskManager.Core.MessageBus;

namespace TaskManager.Application.Events.DeletedTaskEvent;

public class DeletedTaskEventHandler(IMessageBus messageBus) : INotificationHandler<DeletedTaskEventInput>
{
    private readonly IMessageBus _messageBus = messageBus ?? throw new ArgumentNullException(nameof(messageBus));

    public async Task Handle(DeletedTaskEventInput @event, CancellationToken cancellationToken)
    {
        var brokerEvent = new DeletedTaskMessageBusEventInput(@event.Id);
        await _messageBus.PublishAsync(brokerEvent, cancellationToken);
    }
}
