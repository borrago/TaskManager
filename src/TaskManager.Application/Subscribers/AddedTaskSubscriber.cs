using Newtonsoft.Json;
using TaskManager.Application.Events.AddedTaskEvent;
using TaskManager.Core.MessageBus.RabbitMqMessages;
using TaskManager.Infra.TaskRepository.Read;

namespace TaskManager.Application.Subscribers;

public class AddedTaskSubscriber(ITaskRepository taskRepository) : IRabbitMqSubrscriber
{
    private readonly ITaskRepository _taskRepository = taskRepository ?? throw new ArgumentNullException(nameof(taskRepository));

    public async System.Threading.Tasks.Task Handle(string message, CancellationToken cancellationToken)
    {
        var req = JsonConvert.DeserializeObject<AddedTaskEventInput>(message) ?? throw new Exception("Erro ao ler a mensagem.");

        var task = new Domain.ProjectAggregate.Read.Task
        {
            Id = req.Id,
            ProjectId = req.ProjectId,
            Title = req.Title,
            Description = req.Description,
            EndDate = req.EndDate,
            Priority = req.Priority,
            Status = req.Status,
        };

        var inserted = await _taskRepository.InsertAsync(task, cancellationToken);
        if (!inserted)
            throw new Exception("Erro ao inserir o tarefa no banco de leitura.");
    }
}
