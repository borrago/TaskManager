namespace TaskManager.Core.MessageBus;

public interface IMessageBusEventResult
{
    int Status { get; }
    string Message { get; }
}