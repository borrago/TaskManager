namespace TaskManager.Core.MessageBus;

public interface IMessageBus
{
    Task<IMessageBusEventResult> PublishAsync(IMessageBusEventInput @event, CancellationToken cancellationToken);
}