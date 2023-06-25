using Livemedy.Domain.Repositories.Base;
using Livemedy.Domain.Repositories.Users;
using Livemedy.Infrastructure.Repositories.Users;

namespace Livemedy.Infrastructure.Repositories.Base
{
    public class UnitOfWork : IUnitOfWork
    {
        private readonly ApplicationContext _context;

        private IUserRepository _users;
        public UnitOfWork(ApplicationContext context)
        {
            _context = context;
        }
      
        public IUserRepository Users
        {
            get { return _users ??= new UserRepository(_context); }
        }

        public int SaveChanges()
        {
            throw new NotImplementedException();
        }

        public Task<int> SaveChangesAsync()
        {
            throw new NotImplementedException();
        }
    }
}
