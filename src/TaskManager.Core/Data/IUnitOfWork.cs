namespace TaskManager.Core.Data;

public interface IUnitOfWork
{
    Task<bool> CommitAsync(CancellationToken cancellationToken);
}