namespace TaskManager.Application.Commands.AddTaskCommand;

public class AddTaskCommandResult(Guid id)
{
    public Guid Id { get; } = id;
}
