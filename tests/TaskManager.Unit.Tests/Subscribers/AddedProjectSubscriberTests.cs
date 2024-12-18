using Moq;
using Newtonsoft.Json;
using TaskManager.Application.Events.AddedProjectEvent;
using TaskManager.Application.Subscribers;
using TaskManager.Domain.ProjectAggregate.Read;
using TaskManager.Infra.ProjectRepository.Read;

namespace TaskManager.Unit.Tests.Subscribers;

public class AddedProjectSubscriberTests
{
    private readonly Mock<IProjectRepository> _repository;
    private readonly AddedProjectSubscriber _handler;

    public AddedProjectSubscriberTests()
    {
        _repository = new Mock<IProjectRepository>();
        _handler = new AddedProjectSubscriber(_repository.Object);
    }

    [Fact]
    public async System.Threading.Tasks.Task Handle_ShouldInsertClienteClienteModel()
    {
        // Arrange
        var cancellationToken = new CancellationToken();
        var @event = new AddedProjectEventInput(Guid.NewGuid(), "Teste 01", "Teste 01", Guid.NewGuid());
        var model = new Project { Id = @event.Id, Name = "Teste 01", Description = "Teste 01", UserId = Guid.NewGuid() };

        _repository
            .Setup(r => r.InsertAsync(It.IsAny<Project>(), It.IsAny<CancellationToken>()))
            .ReturnsAsync(true);

        // Act
        await _handler.Handle(JsonConvert.SerializeObject(@event), cancellationToken);

        // Assert
        _repository.Verify(r => r.InsertAsync(It.Is<Project>(c =>
            c.Id == model.Id), cancellationToken), Times.Once);
    }
}
