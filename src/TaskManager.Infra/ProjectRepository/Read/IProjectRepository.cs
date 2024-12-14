using TaskManager.Core.Infra;
using TaskManager.Domain.UserAggregate.Read;

namespace TaskManager.Infra.ProjectRepository.Read;

public interface IProjectRepository : IGenericReadRepository<Project>
{
}