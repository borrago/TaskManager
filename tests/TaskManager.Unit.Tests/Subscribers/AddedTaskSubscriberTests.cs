using Moq;
using Newtonsoft.Json;
using TaskManager.Application.Events.AddedTaskEvent;
using TaskManager.Application.Subscribers;
using TaskManager.Infra.TaskRepository.Read;

namespace TaskManager.Unit.Tests.Subscribers;

public class AddedTaskSubscriberTests
{
    private readonly Mock<ITaskRepository> _repository;
    private readonly AddedTaskSubscriber _handler;

    public AddedTaskSubscriberTests()
    {
        _repository = new Mock<ITaskRepository>();
        _handler = new AddedTaskSubscriber(_repository.Object);
    }

    [Fact]
    public async System.Threading.Tasks.Task Handle_ShouldInsertClienteClienteModel()
    {
        // Arrange
        var cancellationToken = new CancellationToken();
        var @event = new AddedTaskEventInput(Guid.NewGuid(), Guid.NewGuid(), Guid.NewGuid(), "Teste 01", "Teste 01", DateTime.Now, "ok", "ok");
        var model = new Domain.ProjectAggregate.Read.Task { Id = @event.Id, Title = "Teste 01", Description = "Teste 01" };

        _repository
            .Setup(r => r.InsertAsync(It.IsAny<Domain.ProjectAggregate.Read.Task>(), It.IsAny<CancellationToken>()))
            .ReturnsAsync(true);

        // Act
        await _handler.Handle(JsonConvert.SerializeObject(@event), cancellationToken);

        // Assert
        _repository.Verify(r => r.InsertAsync(It.Is<Domain.ProjectAggregate.Read.Task>(c =>
            c.Id == model.Id), cancellationToken), Times.Once);
    }
}
