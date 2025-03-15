using Domain.Entities;
using Domain.Repositories;

namespace Infrastructure.Data.Repositories;

public class ServiceRepository(ApplicationDbContext context) : GenericRepository<Service>(context), IServiceRepository { }
