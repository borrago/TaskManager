using TaskManager.Domain.ProjectAggregate.Write;

namespace TaskManager.API.Requests;

public class UpdateTaskRequest
{
    public string Title { get; set; } = "";
    public string Description { get; set; } = "";
    public DateTime EndDate { get; set; }
    public TaskStatusEnum Status { get; set; }
}
