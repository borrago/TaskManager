using TaskManager.Core.DomainObjects;

namespace TaskManager.Domain.UserAggregate.Write;

public class Project : Entity
{
    public string Name { get; private set; } = "";
    public Guid UserId { get; private set; }
    public User User { get; private set; } = null!;
    public IEnumerable<Task> Tasks { get; private set; } = null!;

    // EF Core
    protected Project()
    {
    }

    public Project(string name, Guid userId)
    {
        WithName(name);
        WithUser(userId);
    }

    public void WithName(string name)
    {
        Validations.ValidarSeVazio(name, "O campo Nome não pode estar vazio.");
        Validations.ValidarSeNulo(name, "O campo Nome não pode ser nulo.");
        Validations.ValidarTamanho(name, 250, "O campo Nome não pode ser maior que 250 caracteres.");
        Name = name;
    }

    public void WithUser(Guid userId)
    {
        Validations.ValidarSeNulo(userId, "O campo Usuário não pode ser nulo.");
        UserId = userId;
    }
}
