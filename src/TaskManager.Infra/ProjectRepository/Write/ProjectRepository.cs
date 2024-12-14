using TaskManager.Core.Infra;
using TaskManager.Domain.UserAggregate.Write;

namespace TaskManager.Infra.ProjectRepository.Write;

public class ProjectRepository(Context context) : GenericRepository<Project>(context), IProjectRepository
{
}