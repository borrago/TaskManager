using Newtonsoft.Json;
using TaskManager.Application.Events.DeleteProjectEvent;
using TaskManager.Core.MessageBus.RabbitMqMessages;
using TaskManager.Infra.ProjectRepository.Read;

namespace TaskManager.Application.Subscribers;

public class DeletedProjectSubscriber(IProjectRepository projectRepository) : IRabbitMqSubrscriber
{
    private readonly IProjectRepository _projectRepository = projectRepository ?? throw new ArgumentNullException(nameof(projectRepository));

    public async System.Threading.Tasks.Task Handle(string message, CancellationToken cancellationToken)
    {
        var req = JsonConvert.DeserializeObject<DeletedProjectEventInput>(message) ?? throw new Exception("Erro ao ler a mensagem.");

        var removed = await _projectRepository.RemoveAsync(req.Id, cancellationToken);
        if (!removed)
            throw new Exception("Erro ao remover o projeto no banco de leitura.");
    }
}
