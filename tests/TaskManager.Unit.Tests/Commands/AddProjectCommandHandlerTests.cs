using MediatR;
using Moq;
using TaskManager.Application.Commands.AddProjectCommand;
using TaskManager.Application.Events.AddedProjectEvent;
using TaskManager.Core.Data;
using TaskManager.Domain.ProjectAggregate.Write;
using TaskManager.Infra.ProjectRepository.Write;

namespace TaskManager.Unit.Tests.Commands;

public class AddProjectCommandHandlerTests
{
    private readonly Mock<IProjectRepository> _repositoryMock;
    private readonly Mock<IMediator> _mediatorMock;
    private readonly AddProjectCommandHandler _handler;
    private readonly Mock<IUnitOfWork> _unitOfWorkMock;

    public AddProjectCommandHandlerTests()
    {
        _repositoryMock = new Mock<IProjectRepository>();
        _mediatorMock = new Mock<IMediator>();
        _unitOfWorkMock = new Mock<IUnitOfWork>();
        _repositoryMock.Setup(r => r.UnitOfWork).Returns(_unitOfWorkMock.Object);
        _handler = new AddProjectCommandHandler(_repositoryMock.Object, _mediatorMock.Object);
    }

    [Fact]
    public async System.Threading.Tasks.Task Handle_ShouldAddProjectAndPublishEvent()
    {
        // Arrange
        var cancellationToken = new CancellationToken();
        var command = new AddProjectCommandInput("Task 01", "Task 01", Guid.NewGuid());

        // Act
        await _handler.Handle(command, cancellationToken);

        // Assert
        _repositoryMock.Verify(x => x.AddAsync(It.IsAny<Project>(), cancellationToken), Times.Once);
        _repositoryMock.Verify(x => x.UnitOfWork.CommitAsync(cancellationToken), Times.Once);
        _mediatorMock.Verify(x => x.Publish(It.IsAny<AddedProjectEventInput>(), cancellationToken), Times.Once);
    }
}
