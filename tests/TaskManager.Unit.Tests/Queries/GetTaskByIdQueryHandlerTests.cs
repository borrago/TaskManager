using MediatR;
using Moq;
using System.Linq.Expressions;
using TaskManager.Application.Queries.GetTaskByIdQuery;
using TaskManager.Core.Data;
using TaskManager.Infra.TaskRepository.Read;

namespace TaskManager.Unit.Tests.Queries;

public class GetTaskByIdQueryHandlerTests
{
    private readonly Mock<ITaskRepository> _repositoryMock;
    private readonly Mock<IMediator> _mediatorMock;
    private readonly GetTaskByIdQueryHandler _handler;
    private readonly Mock<IUnitOfWork> _unitOfWorkMock;

    public GetTaskByIdQueryHandlerTests()
    {
        _repositoryMock = new Mock<ITaskRepository>();
        _mediatorMock = new Mock<IMediator>();
        _unitOfWorkMock = new Mock<IUnitOfWork>();
        _handler = new GetTaskByIdQueryHandler(_repositoryMock.Object);
    }

    [Fact]
    public async Task Handle_ShouldReturnTask()
    {
        // Arrange
        var cancellationToken = new CancellationToken();
        var obj = new Domain.ProjectAggregate.Read.Task { Id = Guid.NewGuid(), Description = "Teste 01", Title = "Teste 01" , AssignedUserId = Guid.NewGuid(), Status = "ok", Priority = "ok", EndDate = DateTime.Today.AddDays(3) };
        var query = new GetTaskByIdQueryInput(obj.Id);

        _repositoryMock
                .Setup(r => r.GetByIdAsync(obj.Id, cancellationToken))
                .Returns(Task.FromResult(obj));

        // Act
        var result = await _handler.Handle(query, cancellationToken);

        // Assert
        Assert.Equal(obj.Id, result.Id);
    }
}