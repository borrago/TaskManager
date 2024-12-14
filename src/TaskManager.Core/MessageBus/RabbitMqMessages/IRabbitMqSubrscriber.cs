namespace TaskManager.Core.MessageBus.RabbitMqMessages;

public interface IRabbitMqSubrscriber
{
    public Task Handle(string message, CancellationToken cancellationToken);
}
