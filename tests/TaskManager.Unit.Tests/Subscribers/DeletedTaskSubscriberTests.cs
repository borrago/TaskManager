using Moq;
using Newtonsoft.Json;
using TaskManager.Application.Events.DeletedTaskEvent;
using TaskManager.Application.Subscribers;
using TaskManager.Infra.TaskRepository.Read;

namespace TaskManager.Unit.Tests.Subscribers;

public class DeletedTaskSubscriberTests
{
    private readonly Mock<ITaskRepository> _repository;
    private readonly DeletedTaskSubscriber _handler;

    public DeletedTaskSubscriberTests()
    {
        _repository = new Mock<ITaskRepository>();
        _handler = new DeletedTaskSubscriber(_repository.Object);
    }

    [Fact]
    public async Task Handle_ShouldRemoveTask()
    {
        // Arrange
        var cancellationToken = new CancellationToken();
        var taskEvent = new DeletedTaskEventInput(Guid.NewGuid());
        var serializedMessage = JsonConvert.SerializeObject(taskEvent);

        _repository
            .Setup(r => r.RemoveAsync(taskEvent.Id, cancellationToken))
            .ReturnsAsync(true);

        // Act
        await _handler.Handle(serializedMessage, cancellationToken);

        // Assert
        _repository.Verify(r => r.RemoveAsync(taskEvent.Id, cancellationToken), Times.Once);
    }
}
