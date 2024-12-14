namespace TaskManager.Core.Data;

public interface IRepository
{
    public IUnitOfWork UnitOfWork { get; }
}