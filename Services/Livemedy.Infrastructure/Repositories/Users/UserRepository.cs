using Livemedy.Core.Base.Repositories;
using Livemedy.Domain.Entities.Users;
using Livemedy.Domain.Repositories.Users;

namespace Livemedy.Infrastructure.Repositories.Users;

public class UserRepository : Repository<User>, IUserRepository
{
    public UserRepository(ApplicationContext context) : base(context)
    {
    }
}
