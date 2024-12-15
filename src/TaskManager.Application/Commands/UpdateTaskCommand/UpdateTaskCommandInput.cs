using MediatR;
using TaskManager.Domain.ProjectAggregate.Write;

namespace TaskManager.Application.Commands.UpdateTaskCommand;

public class UpdateTaskCommandInput(Guid id, string title, string description, DateTime endDate, TaskStatusEnum status) : IRequest<UpdateTaskCommandResult>
{
    public Guid Id { get; private set; } = id;
    public string Title { get; private set; } = title;
    public string Description { get; private set; } = description;
    public DateTime EndDate { get; private set; } = endDate;
    public TaskStatusEnum Status { get; private set; } = status;
}
