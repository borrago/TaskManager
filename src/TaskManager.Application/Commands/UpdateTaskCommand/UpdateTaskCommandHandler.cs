using MediatR;
using TaskManager.Application.Events.UpdatedTaskEvent;
using TaskManager.Infra.TaskRepository.Write;

namespace TaskManager.Application.Commands.UpdateTaskCommand;

public class UpdateTaskCommandHandler(ITaskRepository taskRepository, IMediator mediator) : IRequestHandler<UpdateTaskCommandInput, UpdateTaskCommandResult>
{
    private readonly ITaskRepository _projectRepository = taskRepository ?? throw new ArgumentNullException(nameof(taskRepository));
    private readonly IMediator _mediator = mediator ?? throw new ArgumentNullException(nameof(mediator));

    public async Task<UpdateTaskCommandResult> Handle(UpdateTaskCommandInput command, CancellationToken cancellationToken)
    {
        var task = await _projectRepository.GetAsync(g => g.Id == command.Id, cancellationToken) ?? throw new Exception("Tarefa não encontrada.");

        task.WithName(command.Title);
        task.WithDescription(command.Description);
        task.WithEndDate(command.EndDate);
        task.WithStatus(command.Status);

        _projectRepository.Update(task);
        await _projectRepository.UnitOfWork.CommitAsync(cancellationToken);

        var @event = new UpdatedTaskEventInput(task.Id, task.Title, task.Description, task.EndDate, task.Status.ToString());
        await _mediator.Publish(@event, cancellationToken);

        return new UpdateTaskCommandResult(task.Id);
    }
}