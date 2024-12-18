using MediatR;
using Moq;
using System.Linq.Expressions;
using TaskManager.Application.Queries.GetTaskByProjectIdQuery;
using TaskManager.Core.Data;
using TaskManager.Infra.TaskRepository.Read;

namespace TaskManager.Unit.Tests.Queries;

public class GetTaskByProjectIdQueryHandlerTests
{
    private readonly Mock<ITaskRepository> _repositoryMock;
    private readonly Mock<IMediator> _mediatorMock;
    private readonly GetTaskByProjectIdQueryHandler _handler;
    private readonly Mock<IUnitOfWork> _unitOfWorkMock;

    public GetTaskByProjectIdQueryHandlerTests()
    {
        _repositoryMock = new Mock<ITaskRepository>();
        _mediatorMock = new Mock<IMediator>();
        _unitOfWorkMock = new Mock<IUnitOfWork>();
        _handler = new GetTaskByProjectIdQueryHandler(_repositoryMock.Object);
    }

    [Fact]
    public async Task Handle_ShouldReturnTask()
    {
        // Arrange
        var cancellationToken = new CancellationToken();
        var projectId = Guid.NewGuid();
        var query = new GetTaskByProjectIdQueryInput(projectId);

        _repositoryMock
            .Setup(r => r.GetAsync(It.IsAny<Expression<Func<Domain.ProjectAggregate.Read.Task, bool>>>(), It.IsAny<CancellationToken>()))
            .ReturnsAsync((Expression<Func<Domain.ProjectAggregate.Read.Task, bool>> predicate, CancellationToken ct) =>
            {
                var queryable = new List<Domain.ProjectAggregate.Read.Task>
                {
                    new Domain.ProjectAggregate.Read.Task { Id = Guid.NewGuid(), Description = "Teste 01", ProjectId = projectId, Title = "Teste 01" },
                    new Domain.ProjectAggregate.Read.Task { Id = Guid.NewGuid(), Description = "Teste 02", ProjectId = projectId, Title = "Teste 02" }
                };

                return predicate != null
                    ? queryable.AsQueryable().Where(predicate).ToList()
                    : queryable.ToList();
            });

        // Act
        var result = await _handler.Handle(query, cancellationToken);

        // Assert
        Assert.True(result.Items.Count() == 2);
    }
}