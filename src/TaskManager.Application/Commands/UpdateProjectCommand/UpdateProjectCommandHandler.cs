using MediatR;
using TaskManager.Application.Events.UpdatedProjectEvent;
using TaskManager.Infra.ProjectRepository.Write;

namespace TaskManager.Application.Commands.UpdateProjectCommand;

public class UpdateProjectCommandHandler(IProjectRepository projectRepository, IMediator mediator) : IRequestHandler<UpdateProjectCommandInput, UpdateProjectCommandResult>
{
    private readonly IProjectRepository _projectRepository = projectRepository ?? throw new ArgumentNullException(nameof(projectRepository));
    private readonly IMediator _mediator = mediator ?? throw new ArgumentNullException(nameof(mediator));

    public async Task<UpdateProjectCommandResult> Handle(UpdateProjectCommandInput command, CancellationToken cancellationToken)
    {
        var project = await _projectRepository.GetAsync(g => g.Id == command.Id, cancellationToken) ?? throw new Exception("Projeto não encontrado.");

        project.WithName(command.Name);
        project.WithDescription(command.Description);

        _projectRepository.Update(project);
        await _projectRepository.UnitOfWork.CommitAsync(cancellationToken);

        var @event = new UpdatedProjectEventInput(project.Id, project.Name, project.Description);
        await _mediator.Publish(@event, cancellationToken);

        return new UpdateProjectCommandResult(project.Id);
    }
}