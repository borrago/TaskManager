using MediatR;
using TaskManager.Domain.ProjectAggregate.Read;
using TaskManager.Infra.ProjectRepository.Read;

namespace TaskManager.Application.Queries.GetProjectQuery;

public class GetProjectQueryHandler(IProjectRepository projectRepository) : IRequestHandler<GetProjectQueryInput, GetProjectQueryResult>
{
    private readonly IProjectRepository _projectRepository = projectRepository ?? throw new ArgumentNullException(nameof(projectRepository));

    public async Task<GetProjectQueryResult> Handle(GetProjectQueryInput request, CancellationToken cancellationToken)
    {
        var tasks = await _projectRepository.GetAllAsync(cancellationToken);

        return new GetProjectQueryResult
        {
            Items = tasks.Select(s => new Project
            {
                Id = s.Id,
                Name = s.Name,
                Description = s.Description,
                UserId = s.UserId,
            }),
        };
    }
}