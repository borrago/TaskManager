using Microsoft.EntityFrameworkCore;
using System.Reflection;
using TaskManager.Core.Infra;
using TaskManager.Domain.ProjectAggregate.Write;

namespace TaskManager.Infra;

public class Context(DbContextOptions<Context> options) : ContextBase(options)
{
    public DbSet<Project> Projects { get; set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.ApplyConfigurationsFromAssembly(Assembly.GetExecutingAssembly());
        base.OnModelCreating(modelBuilder);
    }
}