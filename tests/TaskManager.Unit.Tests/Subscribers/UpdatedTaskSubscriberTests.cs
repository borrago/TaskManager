using Moq;
using Newtonsoft.Json;
using TaskManager.Application.Events.UpdatedTaskEvent;
using TaskManager.Application.Subscribers;
using TaskManager.Infra.TaskRepository.Read;

namespace TaskManager.Unit.Tests.Subscribers;

public class UpdatedTaskSubscriberTests
{
    private readonly Mock<ITaskRepository> _repository;
    private readonly UpdatedTaskSubscriber _handler;

    public UpdatedTaskSubscriberTests()
    {
        _repository = new Mock<ITaskRepository>();
        _handler = new UpdatedTaskSubscriber(_repository.Object);
    }

    [Fact]
    public async System.Threading.Tasks.Task Handle_ShouldInsertClienteClienteModel()
    {
        // Arrange
        var cancellationToken = new CancellationToken();
        var @event = new UpdatedTaskEventInput(Guid.NewGuid(), "Teste 01", "Teste 01", DateTime.Now, "ok");
        var model = new Domain.ProjectAggregate.Read.Task { Id = @event.Id, Title = "Teste 01", Description = "Teste 01" };

        _repository
            .Setup(r => r.UpdateAsync(It.IsAny<Domain.ProjectAggregate.Read.Task>(), It.IsAny<CancellationToken>()))
            .ReturnsAsync(true);

        // Act
        await _handler.Handle(JsonConvert.SerializeObject(@event), cancellationToken);

        // Assert
        _repository.Verify(r => r.UpdateAsync(It.Is<Domain.ProjectAggregate.Read.Task>(c =>
            c.Id == model.Id), cancellationToken), Times.Once);
    }
}
