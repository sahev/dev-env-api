using Core.Entities;
using Core.Repositories;

namespace Infrastructure.Data.Repositories;

public class UserRepository(ApplicationDbContext context) : GenericRepository<User>(context), IUserRepository { }
