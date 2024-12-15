using MediatR;
using TaskManager.Application.Events.AddedTaskEvent;
using TaskManager.Infra.TaskRepository.Write;

namespace TaskManager.Application.Commands.AddTaskCommand;

public class AddTaskCommandHandler(ITaskRepository taskRepository, IMediator mediator) : IRequestHandler<AddTaskCommandInput, AddTaskCommandResult>
{
    private readonly ITaskRepository _taskRepository = taskRepository ?? throw new ArgumentNullException(nameof(taskRepository));
    private readonly IMediator _mediator = mediator ?? throw new ArgumentNullException(nameof(mediator));

    public async Task<AddTaskCommandResult> Handle(AddTaskCommandInput command, CancellationToken cancellationToken)
    {
        var task = new Domain.ProjectAggregate.Write.Task(command.ProjectId, command.AssignedUserId, command.Title, command.Description, command.EndDate, command.Status, command.Priority);
        await _taskRepository.AddAsync(task, cancellationToken);
        await _taskRepository.UnitOfWork.CommitAsync(cancellationToken);

        var @event = new AddedTaskEventInput(task.Id, task.ProjectId, task.AssignedUserId, task.Title, task.Description, task.EndDate, task.Status.ToString(), task.Priority.ToString());
        await _mediator.Publish(@event, cancellationToken);

        return new AddTaskCommandResult(task.Id);
    }
}