using MediatR;

namespace TaskManager.Application.Commands.DeleteProjectCommand;

public class DeleteProjectCommandInput(Guid id) : IRequest<DeleteProjectCommandResult>
{
    public Guid Id { get; } = id;
}
