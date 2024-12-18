using MongoDB.Driver;
using Moq;
using Newtonsoft.Json;
using TaskManager.Application.Events.DeletedProjectEvent;
using TaskManager.Application.Subscribers;
using TaskManager.Infra.ProjectRepository.Read;
using TaskManager.Infra.TaskRepository.Read;

namespace TaskManager.Unit.Tests.Subscribers;

public class DeletedProjectSubscriberTests
{
    private readonly Mock<IProjectRepository> _repository;
    private readonly Mock<ITaskRepository> _taskRepository;
    private readonly DeletedProjectSubscriber _handler;

    public DeletedProjectSubscriberTests()
    {
        _repository = new Mock<IProjectRepository>();
        _taskRepository = new Mock<ITaskRepository>();
        _handler = new DeletedProjectSubscriber(_repository.Object, _taskRepository.Object);
    }

    [Fact]
    public async System.Threading.Tasks.Task Handle_ShouldRemoveTasksAndProject()
    {
        // Arrange
        var cancellationToken = new CancellationToken();
        var @event = new DeletedProjectEventInput(Guid.NewGuid());
        var serializedEvent = JsonConvert.SerializeObject(@event);

        var taskFilter = Builders<Domain.ProjectAggregate.Read.Task>
                            .Filter.Eq(t => t.ProjectId, @event.Id);

        _taskRepository
            .Setup(r => r.RemoveManyAsync(It.IsAny<FilterDefinition<Domain.ProjectAggregate.Read.Task>>(),
                                          It.IsAny<CancellationToken>()))
            .ReturnsAsync(true);

        _repository
            .Setup(r => r.RemoveAsync(@event.Id, cancellationToken))
            .ReturnsAsync(true);

        // Act
        await _handler.Handle(serializedEvent, cancellationToken);

        // Assert
        _taskRepository.Verify(r => r.RemoveManyAsync(
                                It.IsAny<FilterDefinition<Domain.ProjectAggregate.Read.Task>>(),
                                cancellationToken),
                            Times.Once);

        _repository.Verify(r => r.RemoveAsync(@event.Id, cancellationToken), Times.Once);
    }
}
