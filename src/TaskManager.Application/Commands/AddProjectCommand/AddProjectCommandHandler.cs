using MediatR;
using TaskManager.Application.Events.AddedProjectEvent;
using TaskManager.Domain.ProjectAggregate.Write;
using TaskManager.Infra.ProjectRepository.Write;

namespace TaskManager.Application.Commands.AddProjectCommand;

public class AddProjectCommandHandler(IProjectRepository projectRepository, IMediator mediator) : IRequestHandler<AddProjectCommandInput, AddProjectCommandResult>
{
    private readonly IProjectRepository _projectRepository = projectRepository ?? throw new ArgumentNullException(nameof(projectRepository));
    private readonly IMediator _mediator = mediator ?? throw new ArgumentNullException(nameof(mediator));

    public async Task<AddProjectCommandResult> Handle(AddProjectCommandInput command, CancellationToken cancellationToken)
    {
        var project = new Project(command.Name, command.Description, command.UserId);
        await _projectRepository.AddAsync(project, cancellationToken);
        await _projectRepository.UnitOfWork.CommitAsync(cancellationToken);

        var @event = new AddedProjectEventInput(project.Id, project.Name, project.Description, project.UserId);
        await _mediator.Publish(@event, cancellationToken);

        return new AddProjectCommandResult(project.Id);
    }
}