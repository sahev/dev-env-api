using Domain.Entities;
using Domain.Repositories;

namespace Infrastructure.Data.Repositories;

public class UserRepository(ApplicationDbContext context) : GenericRepository<User>(context), IUserRepository { }
