using MongoDB.Driver;
using Newtonsoft.Json;
using TaskManager.Application.Events.DeletedProjectEvent;
using TaskManager.Core.MessageBus.RabbitMqMessages;
using TaskManager.Infra.ProjectRepository.Read;
using TaskManager.Infra.TaskRepository.Read;

namespace TaskManager.Application.Subscribers;

public class DeletedProjectSubscriber(IProjectRepository projectRepository, ITaskRepository taskRepository) : IRabbitMqSubrscriber
{
    private readonly IProjectRepository _projectRepository = projectRepository ?? throw new ArgumentNullException(nameof(projectRepository));
    private readonly ITaskRepository _taskRepository = taskRepository ?? throw new ArgumentNullException(nameof(taskRepository));

    public async System.Threading.Tasks.Task Handle(string message, CancellationToken cancellationToken)
    {
        var req = JsonConvert.DeserializeObject<DeletedProjectEventInput>(message) ?? throw new Exception("Erro ao ler a mensagem.");

        var taskFilter = Builders<Domain.ProjectAggregate.Read.Task>.Filter.Eq(t => t.ProjectId, req.Id);
        var removedTasks = await _taskRepository.RemoveManyAsync(taskFilter, cancellationToken);
        if (!removedTasks)
            throw new Exception("Erro ao remover as tarefas do projeto no banco de leitura.");

        var removed = await _projectRepository.RemoveAsync(req.Id, cancellationToken);
        if (!removed)
            throw new Exception("Erro ao remover o projeto no banco de leitura.");
    }
}
