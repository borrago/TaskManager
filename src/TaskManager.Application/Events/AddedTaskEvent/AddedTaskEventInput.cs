using TaskManager.Core.MessageBus;

namespace TaskManager.Application.Events.AddedTaskEvent;

public class AddedTaskEventInput(Guid id, Guid projectId, string title, string description, DateTime endDate, string status, string priority) : IEventInput
{
    public Guid Id { get; private set; } = id;
    public Guid ProjectId { get; private set; } = projectId;
    public string Title { get; private set; } = title;
    public string Description { get; private set; } = description;
    public DateTime EndDate { get; private set; } = endDate;
    public string Status { get; private set; } = status;
    public string Priority { get; private set; } = priority;
}
