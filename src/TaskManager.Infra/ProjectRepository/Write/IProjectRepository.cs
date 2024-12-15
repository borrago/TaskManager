using TaskManager.Core.Infra;
using TaskManager.Domain.ProjectAggregate.Write;

namespace TaskManager.Infra.ProjectRepository.Write;

public interface IProjectRepository : IGenericRepository<Project>
{
}