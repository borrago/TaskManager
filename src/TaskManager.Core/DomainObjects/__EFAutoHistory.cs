using Microsoft.EntityFrameworkCore;

namespace TaskManager.Core.DomainObjects;

public class __EFAutoHistory : AutoHistory
{
    public Guid UserId { get; set; }
}