namespace TaskManager.Core.MessageBus;

public class MessageBusEventResult(int status, string message) : IMessageBusEventResult
{
    public int Status { get; } = status;
    public string Message { get; } = message;
}