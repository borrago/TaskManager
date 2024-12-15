using Newtonsoft.Json;
using TaskManager.Application.Events.UpdatedTaskEvent;
using TaskManager.Core.MessageBus.RabbitMqMessages;
using TaskManager.Infra.TaskRepository.Read;

namespace TaskManager.Application.Subscribers;

public class UpdatedTaskSubscriber(ITaskRepository taskRepository) : IRabbitMqSubrscriber
{
    private readonly ITaskRepository _taskRepository = taskRepository ?? throw new ArgumentNullException(nameof(taskRepository));

    public async System.Threading.Tasks.Task Handle(string message, CancellationToken cancellationToken)
    {
        var req = JsonConvert.DeserializeObject<UpdatedTaskEventInput>(message) ?? throw new Exception("Erro ao ler a mensagem.");

        var task = new Domain.ProjectAggregate.Read.Task
        {
            Id = req.Id,
            Title = req.Title,
            Description = req.Description,
            EndDate = req.EndDate,
            Status = req.Status,
        };

        var updated = await _taskRepository.UpdateAsync(task, cancellationToken);
        if (!updated)
            throw new Exception("Erro ao atualizar o tarefa no banco de leitura.");
    }
}
