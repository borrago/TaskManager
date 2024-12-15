using MediatR;
using TaskManager.Application.Events.DeletedTaskEvent;
using TaskManager.Infra.TaskRepository.Write;

namespace TaskManager.Application.Commands.DeleteTaskCommand;

public class DeleteTaskCommandHandler(ITaskRepository taskRepository, IMediator mediator) : IRequestHandler<DeleteTaskCommandInput, DeleteTaskCommandResult>
{
    private readonly ITaskRepository _taskRepository = taskRepository ?? throw new ArgumentNullException(nameof(taskRepository));
    private readonly IMediator _mediator = mediator ?? throw new ArgumentNullException(nameof(mediator));

    public async Task<DeleteTaskCommandResult> Handle(DeleteTaskCommandInput command, CancellationToken cancellationToken)
    {
        var task = await _taskRepository.GetAsync(g => g.Id == command.Id, cancellationToken) ?? throw new Exception("Task não encontrada.");

        _taskRepository.Remove(task);
        await _taskRepository.UnitOfWork.CommitAsync(cancellationToken);

        var @event = new DeletedTaskEventInput(task.Id);
        await _mediator.Publish(@event, cancellationToken);

        return new DeleteTaskCommandResult(task.Id);
    }
}