using MediatR;

namespace TaskManager.Application.Commands.AddProjectCommand;

public class AddProjectCommandInput(string name, string description, Guid userId) : IRequest<AddProjectCommandResult>
{
    public string Name { get; private set; } = name;
    public string Description { get; private set; } = description;
    public Guid UserId { get; private set; } = userId;
}
