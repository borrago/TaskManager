using MediatR;
using TaskManager.Core.MessageBus;

namespace TaskManager.Application.Events.UpdatedProjectEvent;

public class UpdatedProjectEventHandler(IMessageBus messageBus) : INotificationHandler<UpdatedProjectEventInput>
{
    private readonly IMessageBus _messageBus = messageBus ?? throw new ArgumentNullException(nameof(messageBus));

    public async Task Handle(UpdatedProjectEventInput @event, CancellationToken cancellationToken)
    {
        var brokerEvent = new UpdatedProjectMessageBusEventInput(@event.Id, @event.Name, @event.Description);
        await _messageBus.PublishAsync(brokerEvent, cancellationToken);
    }
}
