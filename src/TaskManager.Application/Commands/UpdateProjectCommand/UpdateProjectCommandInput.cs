using MediatR;

namespace TaskManager.Application.Commands.UpdateProjectCommand;

public class UpdateProjectCommandInput(Guid id, string name, string description) : IRequest<UpdateProjectCommandResult>
{
    public Guid Id { get; } = id;
    public string Name { get; } = name;
    public string Description { get; } = description;
}
