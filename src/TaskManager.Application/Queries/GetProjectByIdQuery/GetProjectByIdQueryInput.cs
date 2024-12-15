using MediatR;

namespace TaskManager.Application.Queries.GetProjectByIdQuery;

public class GetProjectByIdQueryInput(Guid id) : IRequest<GetProjectByIdQueryResult>
{
    public Guid Id { get; } = id;
}