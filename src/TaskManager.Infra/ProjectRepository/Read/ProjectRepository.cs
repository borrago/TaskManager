﻿using MongoDB.Driver;
using TaskManager.Core.Infra;
using TaskManager.Domain.UserAggregate.Read;

namespace TaskManager.Infra.ProjectRepository.Read;

public class ProjectRepository(IMongoDatabase database) : GenericReadRepository<Project>(database, "Projects"), IProjectRepository
{
}