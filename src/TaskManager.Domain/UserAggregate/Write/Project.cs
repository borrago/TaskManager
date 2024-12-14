using System.ComponentModel.DataAnnotations;
using TaskManager.Core.DomainObjects;

namespace TaskManager.Domain.UserAggregate.Write;

public class Project : Entity
{
    [MaxLength(250)]
    public string Name { get; private set; } = "";
    [MaxLength(250)]
    public string Description { get; private set; } = "";
    public Guid UserId { get; private set; }
    public IEnumerable<Task> Tasks { get; private set; } = null!;

    // EF Core
    protected Project()
    {
    }

    public Project(string name, string description, Guid userId)
    {
        WithName(name);
        WithDescription(description);
        WithUser(userId);
    }

    public void WithName(string name)
    {
        Validations.ValidarSeVazio(name, "O campo Nome não pode estar vazio.");
        Validations.ValidarSeNulo(name, "O campo Nome não pode ser nulo.");
        Validations.ValidarTamanho(name, 250, "O campo Nome não pode ser maior que 250 caracteres.");
        Name = name;
    }

    public void WithDescription(string description)
    {
        Validations.ValidarSeVazio(description, "O campo Descrição não pode estar vazio.");
        Validations.ValidarSeNulo(description, "O campo Descrição não pode ser nulo.");
        Validations.ValidarTamanho(description, 250, "O campo Descrição não pode ser maior que 250 caracteres.");
        Description = description;
    }

    public void WithUser(Guid userId)
    {
        Validations.ValidarSeNulo(userId, "O campo Usuário não pode ser nulo.");
        UserId = userId;
    }
}
