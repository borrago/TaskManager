namespace TaskManager.Application.Commands.DeleteTaskCommand;

public class DeleteTaskCommandResult(Guid id)
{
    public Guid Id { get; } = id;
}
