namespace TaskManager.Core.MessageBus;

public interface IMessageBusEventInput
{
    Guid Id { get; }
}