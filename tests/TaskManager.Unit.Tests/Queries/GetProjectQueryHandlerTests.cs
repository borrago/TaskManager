using MediatR;
using Moq;
using TaskManager.Application.Queries.GetProjectQuery;
using TaskManager.Core.Data;
using TaskManager.Infra.ProjectRepository.Read;

namespace TaskManager.Unit.Tests.Queries;

public class GetProjectQueryHandlerTests
{
    private readonly Mock<IProjectRepository> _repositoryMock;
    private readonly Mock<IMediator> _mediatorMock;
    private readonly GetProjectQueryHandler _handler;
    private readonly Mock<IUnitOfWork> _unitOfWorkMock;

    public GetProjectQueryHandlerTests()
    {
        _repositoryMock = new Mock<IProjectRepository>();
        _mediatorMock = new Mock<IMediator>();
        _unitOfWorkMock = new Mock<IUnitOfWork>();
        _handler = new GetProjectQueryHandler(_repositoryMock.Object);
    }

    [Fact]
    public async Task Handle_ShouldReturnProject()
    {
        // Arrange
        var cancellationToken = new CancellationToken();
        var obj = new List<Domain.ProjectAggregate.Read.Project>()
        {
            new()
            {
                Id = Guid.NewGuid(), Description = "Teste 01", Name = "Teste 01", UserId = Guid.NewGuid()
            },
            new()
            {
                Id = Guid.NewGuid(), Description = "Teste 02", Name = "Teste 02", UserId = Guid.NewGuid()
            }
        };
        var query = new GetProjectQueryInput();

        _repositoryMock
                .Setup(r => r.GetAllAsync(cancellationToken))
                .Returns(Task.FromResult(obj));

        // Act
        var result = await _handler.Handle(query, cancellationToken);

        // Assert
        Assert.True(result.Items.Count() == 2);
    }
}