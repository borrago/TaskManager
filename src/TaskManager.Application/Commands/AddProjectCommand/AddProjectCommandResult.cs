namespace TaskManager.Application.Commands.AddProjectCommand;

public class AddProjectCommandResult(Guid id)
{
    public Guid Id { get; } = id;
}
