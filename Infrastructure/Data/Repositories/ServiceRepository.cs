using Core.Entities;
using Core.Repositories;

namespace Infrastructure.Data.Repositories;

public class ServiceRepository(ApplicationDbContext context) : GenericRepository<Service>(context), IServiceRepository { }
