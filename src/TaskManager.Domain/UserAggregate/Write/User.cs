using System.ComponentModel.DataAnnotations;
using TaskManager.Core.DomainObjects;

namespace TaskManager.Domain.UserAggregate.Write;

public class User : Entity, IAggregateRoot
{
    [MaxLength(250)]
    public string Name { get; private set; } = "";
    [MaxLength(250)]
    public string Email { get; private set; } = "";

    // EF Core
    protected User()
    {
    }

    public User(string name, string email)
    {
        WithName(name);
        WithEmail(email);
    }

    public void WithName(string name)
    {
        Validations.ValidarSeVazio(name, "O campo Nome não pode estar vazio.");
        Validations.ValidarSeNulo(name, "O campo Nome não pode ser nulo.");
        Validations.ValidarTamanho(name, 250, "O campo Nome não pode ser maior que 250 caracteres.");
        Name = name;
    }

    public void WithEmail(string email)
    {
        Validations.ValidarSeVazio(email, "O campo email não pode estar vazio.");
        Validations.ValidarSeNulo(email, "O campo email não pode ser nulo.");
        Validations.ValidarTamanho(email, 250, "O campo email não pode ser maior que 250 caracteres.");
        Validations.ValidarEmail(email, "E-mail inválido.");
        Email = email;
    }
}
