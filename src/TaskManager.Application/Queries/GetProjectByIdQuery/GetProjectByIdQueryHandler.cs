using MediatR;
using TaskManager.Infra.ProjectRepository.Read;

namespace TaskManager.Application.Queries.GetProjectByIdQuery;

public class GetProjectByIdQueryHandler(IProjectRepository projectRepository) : IRequestHandler<GetProjectByIdQueryInput, GetProjectByIdQueryResult>
{
    private readonly IProjectRepository _projectRepository = projectRepository ?? throw new ArgumentNullException(nameof(projectRepository));

    public async Task<GetProjectByIdQueryResult> Handle(GetProjectByIdQueryInput request, CancellationToken cancellationToken)
    {
        var client = await _projectRepository.GetByIdAsync(request.Id, cancellationToken) ?? throw new Exception("Cliente não localizado.");

        return new GetProjectByIdQueryResult
        {
            Id = client.Id,
            Name = client.Name,
            Description = client.Description,
            UserId = client.UserId,
            Tasks = client.Tasks,
        };

    }
}