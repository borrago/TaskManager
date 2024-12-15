using Newtonsoft.Json;
using TaskManager.Application.Events.AddedProjectEvent;
using TaskManager.Core.MessageBus.RabbitMqMessages;
using TaskManager.Domain.ProjectAggregate.Read;
using TaskManager.Infra.ProjectRepository.Read;

namespace TaskManager.Application.Subscribers;

public class AddedProjectSubscriber(IProjectRepository projectRepository) : IRabbitMqSubrscriber
{
    private readonly IProjectRepository _projectRepository = projectRepository ?? throw new ArgumentNullException(nameof(projectRepository));

    public async System.Threading.Tasks.Task Handle(string message, CancellationToken cancellationToken)
    {
        var req = JsonConvert.DeserializeObject<AddedProjectEventInput>(message) ?? throw new Exception("Erro ao ler a mensagem.");

        var project = new Project
        {
            Id = req.Id,
            Name = req.Name,
            Description = req.Description,
            UserId = req.UserId,
        };

        var inserted = await _projectRepository.InsertAsync(project, cancellationToken);
        if (!inserted)
            throw new Exception("Erro ao inserir o projeto no banco de leitura.");
    }
}
