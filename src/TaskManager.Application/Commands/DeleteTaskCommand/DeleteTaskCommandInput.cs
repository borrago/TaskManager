using MediatR;

namespace TaskManager.Application.Commands.DeleteTaskCommand;

public class DeleteTaskCommandInput(Guid id) : IRequest<DeleteTaskCommandResult>
{
    public Guid Id { get; } = id;
}
