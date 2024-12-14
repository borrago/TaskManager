using System.ComponentModel.DataAnnotations;
using TaskManager.Core.DomainObjects;

namespace TaskManager.Domain.UserAggregate.Write;

public class Task : Entity
{
    [MaxLength(250)]
    public string Title { get; private set; } = "";
    [MaxLength(250)]
    public string Description { get; private set; } = "";
    public DateTime EndDate { get; private set; }
    public TaskStatusEnum Status { get; private set; }
    public TaskPriorityEnum Priority { get; private set; }
    public Guid ProjectId { get; private set; }
    public Project Project { get; private set; } = null!;

    // EF Core
    protected Task()
    {
    }

    public Task(string title, string description, DateTime endDate, TaskStatusEnum status, TaskPriorityEnum priority)
    {
        WithName(title);
        WithDescription(description);
        WithEndDate(endDate);
        WithStatus(status);
        WithPriority(priority);
    }

    public void WithName(string title)
    {
        Validations.ValidarSeVazio(title, "O campo Titulo não pode estar vazio.");
        Validations.ValidarSeNulo(title, "O campo Titulo não pode ser nulo.");
        Validations.ValidarTamanho(title, 250, "O campo Titulo não pode ser maior que 250 caracteres.");
        Title = title;
    }

    public void WithDescription(string description)
    {
        Validations.ValidarSeVazio(description, "O campo Descrição não pode estar vazio.");
        Validations.ValidarSeNulo(description, "O campo Descrição não pode ser nulo.");
        Validations.ValidarTamanho(description, 250, "O campo Descrição não pode ser maior que 250 caracteres.");
        Description = description;
    }

    public void WithEndDate(DateTime endDate)
    {
        Validations.ValidarSeNulo(endDate, "O campo Data de Término não pode ser nulo.");
        EndDate = endDate;
    }

    public void WithStatus(TaskStatusEnum status)
    {
        Validations.ValidarSeNulo(status, "O campo Status não pode ser nulo.");
        Status = status;
    }

    public void WithPriority(TaskPriorityEnum priority)
    {
        Validations.ValidarSeNulo(priority, "O campo Prioridade não pode ser nulo.");
        Priority = priority;
    }
}
