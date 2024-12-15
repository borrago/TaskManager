using TaskManager.Core.Infra;
using TaskManager.Domain.ProjectAggregate.Read;

namespace TaskManager.Infra.ProjectRepository.Read;

public interface IProjectRepository : IGenericReadRepository<Project>
{
}