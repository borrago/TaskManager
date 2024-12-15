using MediatR;
using TaskManager.Application.Events.DeleteProjectEvent;
using TaskManager.Infra.ProjectRepository.Write;

namespace TaskManager.Application.Commands.DeleteProjectCommand;

public class DeleteProjectCommandHandler(IProjectRepository projectRepository, IMediator mediator) : IRequestHandler<DeleteProjectCommandInput, DeleteProjectCommandResult>
{
    private readonly IProjectRepository _projectRepository = projectRepository ?? throw new ArgumentNullException(nameof(projectRepository));
    private readonly IMediator _mediator = mediator ?? throw new ArgumentNullException(nameof(mediator));

    public async Task<DeleteProjectCommandResult> Handle(DeleteProjectCommandInput command, CancellationToken cancellationToken)
    {
        var project = await _projectRepository.GetAsync(g => g.Id == command.Id, cancellationToken) ?? throw new Exception("Projeto não encontrado.");

        _projectRepository.Remove(project);
        await _projectRepository.UnitOfWork.CommitAsync(cancellationToken);

        var @event = new DeletedProjectEventInput(project.Id);
        await _mediator.Publish(@event, cancellationToken);

        return new DeleteProjectCommandResult(project.Id);
    }
}