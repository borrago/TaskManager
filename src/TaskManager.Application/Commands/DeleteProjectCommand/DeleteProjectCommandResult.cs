namespace TaskManager.Application.Commands.DeleteProjectCommand;

public class DeleteProjectCommandResult(Guid id)
{
    public Guid Id { get; } = id;
}
