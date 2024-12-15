using Newtonsoft.Json;
using TaskManager.Application.Events.UpdatedProjectEvent;
using TaskManager.Core.MessageBus.RabbitMqMessages;
using TaskManager.Domain.ProjectAggregate.Read;
using TaskManager.Infra.ProjectRepository.Read;

namespace TaskManager.Application.Subscribers;

public class UpdatedProjectSubscriber(IProjectRepository projectRepository) : IRabbitMqSubrscriber
{
    private readonly IProjectRepository _projectRepository = projectRepository ?? throw new ArgumentNullException(nameof(projectRepository));

    public async System.Threading.Tasks.Task Handle(string message, CancellationToken cancellationToken)
    {
        var req = JsonConvert.DeserializeObject<UpdatedProjectEventInput>(message) ?? throw new Exception("Erro ao ler a mensagem.");

        var project = new Project
        {
            Id = req.Id,
            Name = req.Name,
            Description = req.Description,
        };

        var updated = await _projectRepository.UpdateAsync(project, cancellationToken);
        if (!updated)
            throw new Exception("Erro ao atualizar o projeto no banco de leitura.");
    }
}
