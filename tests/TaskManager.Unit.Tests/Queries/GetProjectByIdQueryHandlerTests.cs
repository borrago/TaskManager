using MediatR;
using Moq;
using System.Linq.Expressions;
using TaskManager.Application.Queries.GetProjectByIdQuery;
using TaskManager.Core.Data;
using TaskManager.Infra.ProjectRepository.Read;
using TaskManager.Infra.TaskRepository.Read;

namespace TaskManager.Unit.Tests.Queries;

public class GetProjectByIdQueryHandlerTests
{
    private readonly Mock<IProjectRepository> _repositoryMock;
    private readonly Mock<ITaskRepository> _taskRepositoryMock;
    private readonly Mock<IMediator> _mediatorMock;
    private readonly GetProjectByIdQueryHandler _handler;
    private readonly Mock<IUnitOfWork> _unitOfWorkMock;

    public GetProjectByIdQueryHandlerTests()
    {
        _repositoryMock = new Mock<IProjectRepository>();
        _taskRepositoryMock = new Mock<ITaskRepository>();
        _mediatorMock = new Mock<IMediator>();
        _unitOfWorkMock = new Mock<IUnitOfWork>();
        _handler = new GetProjectByIdQueryHandler(_repositoryMock.Object, _taskRepositoryMock.Object);
    }

    [Fact]
    public async Task Handle_ShouldReturnProject()
    {
        // Arrange
        var cancellationToken = new CancellationToken();
        var projectId = Guid.NewGuid();
        var queryable = new List<Domain.ProjectAggregate.Read.Task>
        {
            new Domain.ProjectAggregate.Read.Task { Id = Guid.NewGuid(), Description = "Teste 01", ProjectId = projectId, Title = "Teste 01" },
            new Domain.ProjectAggregate.Read.Task { Id = Guid.NewGuid(), Description = "Teste 02", ProjectId = projectId, Title = "Teste 02" }
        };
        var obj = new Domain.ProjectAggregate.Read.Project { Id = projectId, Description = "Teste 01", Name = "Teste 01", UserId = Guid.NewGuid(), Tasks = queryable };
        var query = new GetProjectByIdQueryInput(obj.Id);

        _repositoryMock
                .Setup(r => r.GetByIdAsync(obj.Id, cancellationToken))
                .Returns(Task.FromResult(obj));

        _taskRepositoryMock
            .Setup(r => r.GetAsync(It.IsAny<Expression<Func<Domain.ProjectAggregate.Read.Task, bool>>>(), It.IsAny<CancellationToken>()))
            .ReturnsAsync((Expression<Func<Domain.ProjectAggregate.Read.Task, bool>> predicate, CancellationToken ct) =>
            {
                return predicate != null
                    ? queryable.AsQueryable().Where(predicate).ToList()
                    : queryable.ToList();
            });


        // Act
        var result = await _handler.Handle(query, cancellationToken);

        // Assert
        Assert.Equal(obj.Id, result.Id);
        Assert.Equal(obj.Tasks.Count(), result.Tasks.Count());
    }
}