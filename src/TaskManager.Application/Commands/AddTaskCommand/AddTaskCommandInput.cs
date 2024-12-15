using MediatR;
using TaskManager.Domain.ProjectAggregate.Write;

namespace TaskManager.Application.Commands.AddTaskCommand;

public class AddTaskCommandInput(Guid projectId, string title, string description, DateTime endDate, TaskStatusEnum status, TaskPriorityEnum priority) : IRequest<AddTaskCommandResult>
{
    public Guid ProjectId { get; private set; } = projectId;
    public string Title { get; private set; } = title;
    public string Description { get; private set; } = description;
    public DateTime EndDate { get; private set; } = endDate;
    public TaskStatusEnum Status { get; private set; } = status;
    public TaskPriorityEnum Priority { get; private set; } = priority;
}
