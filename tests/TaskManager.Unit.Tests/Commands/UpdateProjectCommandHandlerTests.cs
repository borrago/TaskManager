using MediatR;
using Moq;
using System.Linq.Expressions;
using TaskManager.Application.Commands.UpdateProjectCommand;
using TaskManager.Application.Events.UpdatedProjectEvent;
using TaskManager.Core.Data;
using TaskManager.Domain.ProjectAggregate.Write;
using TaskManager.Infra.ProjectRepository.Write;

namespace TaskManager.Unit.Tests.Commands;

public class UpdateProjectCommandHandlerTests
{
    private readonly Mock<IProjectRepository> _repositoryMock;
    private readonly Mock<IMediator> _mediatorMock;
    private readonly UpdateProjectCommandHandler _handler;
    private readonly Mock<IUnitOfWork> _unitOfWorkMock;

    public UpdateProjectCommandHandlerTests()
    {
        _repositoryMock = new Mock<IProjectRepository>();
        _mediatorMock = new Mock<IMediator>();
        _unitOfWorkMock = new Mock<IUnitOfWork>();
        _repositoryMock.Setup(r => r.UnitOfWork).Returns(_unitOfWorkMock.Object);
        _handler = new UpdateProjectCommandHandler(_repositoryMock.Object, _mediatorMock.Object);
    }

    [Fact]
    public async System.Threading.Tasks.Task Handle_ShouldUpdateProjectAndPublishEvent()
    {
        // Arrange
        var cancellationToken = new CancellationToken();
        var project = new Project("Task 01", "Task 01", Guid.NewGuid());
        var command = new UpdateProjectCommandInput(project.Id, "Task 02", "Task 02");

        var queryable = new[] { project }.AsQueryable();

        _repositoryMock
               .Setup(r => r.GetAsync(It.IsAny<Expression<Func<Project, bool>>>(), It.IsAny<CancellationToken>()))
               .Returns((Expression<Func<Project, bool>> predicate, CancellationToken ct) =>
                   System.Threading.Tasks.Task.FromResult(queryable.FirstOrDefault(predicate)));

        // Act
        await _handler.Handle(command, cancellationToken);

        // Assert
        _repositoryMock.Verify(x => x.Update(It.IsAny<Project>()), Times.Once);
        _repositoryMock.Verify(x => x.UnitOfWork.CommitAsync(cancellationToken), Times.Once);
        _mediatorMock.Verify(x => x.Publish(It.IsAny<UpdatedProjectEventInput>(), cancellationToken), Times.Once);
    }
}
