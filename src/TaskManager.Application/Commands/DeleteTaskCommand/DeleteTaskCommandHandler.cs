using MediatR;
using TaskManager.Application.Commands.DeleteProjectCommand;
using TaskManager.Application.Events.DeletedTaskEvent;
using TaskManager.Infra.TaskRepository.Write;

namespace TaskManager.Application.Commands.DeleteTaskCommand;

public class DeleteTaskCommandHandler(ITaskRepository taskRepository, IMediator mediator) : IRequestHandler<DeleteProjectCommandInput, DeleteProjectCommandResult>
{
    private readonly ITaskRepository _taskRepository = taskRepository ?? throw new ArgumentNullException(nameof(taskRepository));
    private readonly IMediator _mediator = mediator ?? throw new ArgumentNullException(nameof(mediator));

    public async Task<DeleteProjectCommandResult> Handle(DeleteProjectCommandInput command, CancellationToken cancellationToken)
    {
        var task = await _taskRepository.GetAsync(g => g.Id == command.Id, cancellationToken) ?? throw new Exception("Tarefa não encontrado.");

        _taskRepository.Remove(task);
        await _taskRepository.UnitOfWork.CommitAsync(cancellationToken);

        var @event = new DeletedTaskEventInput(task.Id);
        await _mediator.Publish(@event, cancellationToken);

        return new DeleteProjectCommandResult(task.Id);
    }
}