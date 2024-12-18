using Moq;
using Newtonsoft.Json;
using TaskManager.Application.Events.UpdatedProjectEvent;
using TaskManager.Application.Subscribers;
using TaskManager.Domain.ProjectAggregate.Read;
using TaskManager.Infra.ProjectRepository.Read;

namespace TaskManager.Unit.Tests.Subscribers;

public class UpdatedProjectSubscriberTests
{
    private readonly Mock<IProjectRepository> _repository;
    private readonly UpdatedProjectSubscriber _handler;

    public UpdatedProjectSubscriberTests()
    {
        _repository = new Mock<IProjectRepository>();
        _handler = new UpdatedProjectSubscriber(_repository.Object);
    }

    [Fact]
    public async System.Threading.Tasks.Task Handle_ShouldInsertClienteClienteModel()
    {
        // Arrange
        var cancellationToken = new CancellationToken();
        var @event = new UpdatedProjectEventInput(Guid.NewGuid(), "Teste 01", "Teste 01");
        var model = new Project { Id = @event.Id, Name = "Teste 01", Description = "Teste 01", UserId = Guid.NewGuid() };

        _repository
            .Setup(r => r.UpdateAsync(It.IsAny<Project>(), It.IsAny<CancellationToken>()))
            .ReturnsAsync(true);

        // Act
        await _handler.Handle(JsonConvert.SerializeObject(@event), cancellationToken);

        // Assert
        _repository.Verify(r => r.UpdateAsync(It.Is<Project>(c =>
            c.Id == model.Id), cancellationToken), Times.Once);
    }
}
