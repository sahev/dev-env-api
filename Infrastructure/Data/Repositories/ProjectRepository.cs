using Core.Entities;
using Core.Repositories;

namespace Infrastructure.Data.Repositories;

public class ProjectRepository(ApplicationDbContext context) : GenericRepository<Project>(context), IProjectRepository { }
