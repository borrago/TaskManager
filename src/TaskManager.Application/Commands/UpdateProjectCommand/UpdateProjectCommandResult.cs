namespace TaskManager.Application.Commands.UpdateProjectCommand;

public class UpdateProjectCommandResult(Guid id)
{
    public Guid Id { get; } = id;
}
