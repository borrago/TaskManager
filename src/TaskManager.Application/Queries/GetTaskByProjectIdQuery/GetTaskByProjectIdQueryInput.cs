using MediatR;

namespace TaskManager.Application.Queries.GetTaskByProjectIdQuery;

public class GetTaskByProjectIdQueryInput(Guid projectId) : IRequest<GetTaskByProjectIdQueryResult>
{
    public Guid ProjectId { get; } = projectId;
}