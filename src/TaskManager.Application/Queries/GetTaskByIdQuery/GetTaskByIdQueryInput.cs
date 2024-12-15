using MediatR;

namespace TaskManager.Application.Queries.GetTaskByIdQuery;

public class GetTaskByIdQueryInput(Guid id) : IRequest<GetTaskByIdQueryResult>
{
    public Guid Id { get; } = id;
}