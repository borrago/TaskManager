using FluentValidation;
using TaskManager.Domain.ProjectAggregate.Write;
using TaskManager.Infra.TaskRepository.Read;

namespace TaskManager.Application.Commands.DeleteProjectCommand;

public class AddTaskCommandValidator : AbstractValidator<DeleteProjectCommandInput>
{
    public AddTaskCommandValidator(ITaskRepository taskRepository)
    {
        RuleFor(x => x.Id)
            .MustAsync(async (projectId, cancellationToken) =>
            {
                var tasks = await taskRepository.GetAsync(t => t.ProjectId == projectId, cancellationToken);
                return !tasks.Any(a => a.Status == TaskStatusEnum.Pendente.ToString());
            })
            .WithMessage("O projeto não pode ser excluído porque possui tarefas pendentes. Conclua ou remova essas tarefas.");

        RuleFor(x => x.Id)
            .MustAsync(async (projectId, cancellationToken) =>
            {
                var tasks = await taskRepository.GetAsync(t => t.ProjectId == projectId, cancellationToken);
                return !tasks.Any(a => a.Status == TaskStatusEnum.EmAndamento.ToString());
            })
            .WithMessage("O projeto não pode ser excluído porque possui tarefas em andamento. Conclua ou remova essas tarefas.");
    }
}