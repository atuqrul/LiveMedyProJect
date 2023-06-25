using Livemedy.Domain.Repositories.Users;

namespace Livemedy.Domain.Repositories.Base
{
    public interface IUnitOfWork
    {
        IUserRepository Users { get; }
       
        int SaveChanges();
        Task<int> SaveChangesAsync();

    }
}
