namespace TaskManager.Application.Commands.UpdateTaskCommand;

public class UpdateTaskCommandResult(Guid id)
{
    public Guid Id { get; } = id;
}
