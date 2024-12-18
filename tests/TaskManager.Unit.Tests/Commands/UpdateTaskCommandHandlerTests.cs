using MediatR;
using Moq;
using System.Linq.Expressions;
using TaskManager.Application.Commands.UpdateTaskCommand;
using TaskManager.Application.Events.UpdatedTaskEvent;
using TaskManager.Core.Data;
using TaskManager.Infra.TaskRepository.Write;

namespace TaskManager.Unit.Tests.Commands;

public class UpdateTaskCommandHandlerTests
{
    private readonly Mock<ITaskRepository> _repositoryMock;
    private readonly Mock<IMediator> _mediatorMock;
    private readonly UpdateTaskCommandHandler _handler;
    private readonly Mock<IUnitOfWork> _unitOfWorkMock;

    public UpdateTaskCommandHandlerTests()
    {
        _repositoryMock = new Mock<ITaskRepository>();
        _mediatorMock = new Mock<IMediator>();
        _unitOfWorkMock = new Mock<IUnitOfWork>();
        _repositoryMock.Setup(r => r.UnitOfWork).Returns(_unitOfWorkMock.Object);
        _handler = new UpdateTaskCommandHandler(_repositoryMock.Object, _mediatorMock.Object);
    }

    [Fact]
    public async System.Threading.Tasks.Task Handle_ShouldUpdateProjectAndPublishEvent()
    {
        // Arrange
        var cancellationToken = new CancellationToken();
        var project = new Domain.ProjectAggregate.Write.Task(Guid.NewGuid(), Guid.NewGuid(), "Task 01", "Task 01",
            DateTime.Now.AddDays(3), Domain.ProjectAggregate.Write.TaskStatusEnum.EmAndamento, Domain.ProjectAggregate.Write.TaskPriorityEnum.Alta);
        var command = new UpdateTaskCommandInput(project.Id, "Task 02", "Task 02",
            DateTime.Now.AddDays(3), Domain.ProjectAggregate.Write.TaskStatusEnum.Concluida);

        var queryable = new[] { project }.AsQueryable();

        _repositoryMock
               .Setup(r => r.GetAsync(It.IsAny<Expression<Func<Domain.ProjectAggregate.Write.Task, bool>>>(), It.IsAny<CancellationToken>()))
               .Returns((Expression<Func<Domain.ProjectAggregate.Write.Task, bool>> predicate, CancellationToken ct) =>
                   System.Threading.Tasks.Task.FromResult(queryable.FirstOrDefault(predicate)));

        // Act
        await _handler.Handle(command, cancellationToken);

        // Assert
        _repositoryMock.Verify(x => x.Update(It.IsAny<Domain.ProjectAggregate.Write.Task>()), Times.Once);
        _repositoryMock.Verify(x => x.UnitOfWork.CommitAsync(cancellationToken), Times.Once);
        _mediatorMock.Verify(x => x.Publish(It.IsAny<UpdatedTaskEventInput>(), cancellationToken), Times.Once);
    }
}
