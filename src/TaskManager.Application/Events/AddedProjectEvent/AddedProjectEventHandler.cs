using MediatR;
using TaskManager.Core.MessageBus;

namespace TaskManager.Application.Events.AddedProjectEvent;

public class AddedProjectEventHandler(IMessageBus messageBus) : INotificationHandler<AddedProjectEventInput>
{
    private readonly IMessageBus _messageBus = messageBus ?? throw new ArgumentNullException(nameof(messageBus));

    public async Task Handle(AddedProjectEventInput @event, CancellationToken cancellationToken)
    {
        var brokerEvent = new AddedProjectMessageBusEventInput(@event.Id, @event.Name, @event.Description, @event.UserId);
        await _messageBus.PublishAsync(brokerEvent, cancellationToken);
    }
}
