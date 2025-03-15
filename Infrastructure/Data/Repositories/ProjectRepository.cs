using Domain.Entities;
using Domain.Repositories;

namespace Infrastructure.Data.Repositories;

public class ProjectRepository(ApplicationDbContext context) : GenericRepository<Project>(context), IProjectRepository { }
