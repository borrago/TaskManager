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
        var project = await _projectRepository.GetAsync(g => g.Id == command.Id, cancellationToken) ?? throw new Exception("Tarefa não encontrada.");

        project.WithName(command.Title);
        project.WithDescription(command.Description);
        project.WithEndDate(command.EndDate);
        project.WithStatus(command.Status);

        _projectRepository.Update(project);
        await _projectRepository.UnitOfWork.CommitAsync(cancellationToken);

        var @event = new UpdatedTaskEventInput(project.Id, project.Title, project.Description, project.EndDate, project.Status.ToString());
        await _mediator.Publish(@event, cancellationToken);

        return new UpdateTaskCommandResult(project.Id);
    }
}