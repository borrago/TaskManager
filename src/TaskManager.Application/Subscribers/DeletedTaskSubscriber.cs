using Newtonsoft.Json;
using TaskManager.Application.Events.DeletedTaskEvent;
using TaskManager.Core.MessageBus.RabbitMqMessages;
using TaskManager.Infra.TaskRepository.Read;

namespace TaskManager.Application.Subscribers;

public class DeletedTaskSubscriber(ITaskRepository taskRepository) : IRabbitMqSubrscriber
{
    private readonly ITaskRepository _taskRepository = taskRepository ?? throw new ArgumentNullException(nameof(taskRepository));

    public async System.Threading.Tasks.Task Handle(string message, CancellationToken cancellationToken)
    {
        var req = JsonConvert.DeserializeObject<DeletedTaskEventInput>(message) ?? throw new Exception("Erro ao ler a mensagem.");

        var removed = await _taskRepository.RemoveAsync(req.Id, cancellationToken);
        if (!removed)
            throw new Exception("Erro ao remover o tarefa no banco de leitura.");
    }
}
