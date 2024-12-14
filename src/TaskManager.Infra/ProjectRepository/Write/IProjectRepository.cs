using TaskManager.Core.Infra;
using TaskManager.Domain.UserAggregate.Write;

namespace TaskManager.Infra.ProjectRepository.Write;

public interface IProjectRepository : IGenericRepository<Project>
{
}