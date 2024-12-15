using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using TaskManager.Core.Data;
using TaskManager.Core.DomainObjects;

namespace TaskManager.Core.Infra;

public abstract class ContextBase(DbContextOptions options) : DbContext(options), IUnitOfWork
{
    private static readonly ILoggerFactory _loggerFactory = LoggerFactory.Create(
        builder =>
        {
            builder
                .AddFilter((category, level) =>
                    category == DbLoggerCategory.Database.Command.Name
                    && level == LogLevel.Information);
            builder.AddConsole();
        });

    public async Task<bool> CommitAsync(CancellationToken cancellationToken = default)
    {
        this.EnsureAutoHistory(() => new __EFAutoHistory()
        {
            UserId = Guid.Parse("b4b0f153-cd42-4e3a-ac40-9f435244f0c6"),
        });

        return await SaveChangesAsync(cancellationToken) > 0;
    }

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        => optionsBuilder.UseLoggerFactory(_loggerFactory);

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.EnableAutoHistory<__EFAutoHistory>(o => { });

        base.OnModelCreating(modelBuilder);
    }
}
