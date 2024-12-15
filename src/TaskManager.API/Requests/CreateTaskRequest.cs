using TaskManager.Domain.ProjectAggregate.Write;

namespace TaskManager.API.Requests;

public class CreateTaskRequest
{
    public Guid ProjectId { get; set; }
    public Guid AssignedUserId { get; set; }
    public string Title { get; set; } = "";
    public string Description { get; set; } = "";
    public DateTime EndDate { get; set; }
    public TaskStatusEnum Status { get; set; }
    public TaskPriorityEnum Priority { get; set; }
}
