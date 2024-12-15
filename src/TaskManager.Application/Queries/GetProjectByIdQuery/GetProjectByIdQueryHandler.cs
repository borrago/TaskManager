using MediatR;
using TaskManager.Infra.ProjectRepository.Read;
using TaskManager.Infra.TaskRepository.Read;

namespace TaskManager.Application.Queries.GetProjectByIdQuery;

public class GetProjectByIdQueryHandler(IProjectRepository projectRepository, ITaskRepository taskRepository) : IRequestHandler<GetProjectByIdQueryInput, GetProjectByIdQueryResult>
{
    private readonly IProjectRepository _projectRepository = projectRepository ?? throw new ArgumentNullException(nameof(projectRepository));
    private readonly ITaskRepository _taskRepository = taskRepository ?? throw new ArgumentNullException(nameof(taskRepository));

    public async Task<GetProjectByIdQueryResult> Handle(GetProjectByIdQueryInput request, CancellationToken cancellationToken)
    {
        var project = await _projectRepository.GetByIdAsync(request.Id, cancellationToken) ?? throw new ApplicationException("Projeto não localizado.");
        var tasks = await _taskRepository.GetAsync(g => g.ProjectId == project.Id, cancellationToken);

        return new GetProjectByIdQueryResult
        {
            Id = project.Id,
            Name = project.Name,
            Description = project.Description,
            Tasks = tasks.Select(s => new GetTaskByIdQuery.GetTaskByIdQueryResult
            {
                Id = s.Id,
                ProjectId = project.Id,
                Title = s.Title,
                Description = s.Description,
                EndDate = s.EndDate,
                Priority = s.Priority,
                Status = s.Status,
            }),
            UserId = project.UserId,
        };
    }
}