using MediatR;
using TaskManager.Core.MessageBus;

namespace TaskManager.Application.Events.DeleteProjectEvent;

public class DeletedProjectEventHandler(IMessageBus messageBus) : INotificationHandler<DeletedProjectEventInput>
{
    private readonly IMessageBus _messageBus = messageBus ?? throw new ArgumentNullException(nameof(messageBus));

    public async Task Handle(DeletedProjectEventInput @event, CancellationToken cancellationToken)
    {
        var brokerEvent = new DeletedProjectMessageBusEventInput(@event.Id);
        await _messageBus.PublishAsync(brokerEvent, cancellationToken);
    }
}
