using FluentValidation;
using TaskManager.Application.Commands.AddTaskCommand;
using TaskManager.Infra.TaskRepository.Read;

namespace TaskManager.Application.Commands.AddTaskAddTaskCommand;

public class AddTaskCommandValidator : AbstractValidator<AddTaskCommandInput>
{
    public AddTaskCommandValidator(ITaskRepository taskRepository)
    {
        RuleFor(x => x.ProjectId)
            .MustAsync(async (projectId, cancellationToken) =>
            {
                var tasks = await taskRepository.GetAsync(g => g.ProjectId == projectId, cancellationToken);
                return tasks.Count < 20;
            })
            .WithMessage("O projeto não pode possuir mais de 20 tarefas.");
    }
}