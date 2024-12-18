using MediatR;
using Moq;
using TaskManager.Application.Commands.AddTaskCommand;
using TaskManager.Application.Events.AddedTaskEvent;
using TaskManager.Core.Data;
using TaskManager.Domain.ProjectAggregate.Write;
using TaskManager.Infra.TaskRepository.Write;

namespace TaskManager.Unit.Tests.Commands;

public class AddTaskCommandHandlerTests
{
    private readonly Mock<ITaskRepository> _repositoryMock;
    private readonly Mock<IMediator> _mediatorMock;
    private readonly AddTaskCommandHandler _handler;
    private readonly Mock<IUnitOfWork> _unitOfWorkMock;

    public AddTaskCommandHandlerTests()
    {
        _repositoryMock = new Mock<ITaskRepository>();
        _mediatorMock = new Mock<IMediator>();
        _unitOfWorkMock = new Mock<IUnitOfWork>();
        _repositoryMock.Setup(r => r.UnitOfWork).Returns(_unitOfWorkMock.Object);
        _handler = new AddTaskCommandHandler(_repositoryMock.Object, _mediatorMock.Object);
    }

    [Fact]
    public async System.Threading.Tasks.Task Handle_ShouldAddProjectAndPublishEvent()
    {
        // Arrange
        var cancellationToken = new CancellationToken();
        var command = new AddTaskCommandInput(Guid.NewGuid(), Guid.NewGuid(), "Task 01", "Task 01", DateTime.Now.AddDays(3), TaskStatusEnum.Pendente, TaskPriorityEnum.Alta);

        // Act
        await _handler.Handle(command, cancellationToken);

        // Assert
        _repositoryMock.Verify(x => x.AddAsync(It.IsAny<Domain.ProjectAggregate.Write.Task>(), cancellationToken), Times.Once);
        _repositoryMock.Verify(x => x.UnitOfWork.CommitAsync(cancellationToken), Times.Once);
        _mediatorMock.Verify(x => x.Publish(It.IsAny<AddedTaskEventInput>(), cancellationToken), Times.Once);
    }
}
