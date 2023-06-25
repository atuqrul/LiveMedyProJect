using Livemedy.Core.Base.Repositories;
using Livemedy.Domain.Entities.Users;
using Livemedy.Domain.Repositories.Users;

namespace Livemedy.Infrastructure.Repositories.Users;

public class RoleRepository : Repository<Role>, IRoleRepository
{
    public RoleRepository(ApplicationContext context) : base(context)
    {
    }
}
