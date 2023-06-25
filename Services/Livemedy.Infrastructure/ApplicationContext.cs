using Livemedy.Core.Base;
using Livemedy.Domain.Entities.Users;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;

namespace Livemedy.Infrastructure
{
    public class ApplicationContext : BaseContext
    {
        
        public ApplicationContext(DbContextOptions options, IHttpContextAccessor httpContextAccessor) : base(options, httpContextAccessor)
        {
           
        }
        public DbSet<User> User { get; set; }
        public DbSet<Role> Role { get; set; }

        protected override void OnModelCreating(ModelBuilder builder)        {
            

            base.OnModelCreating(builder);
        }
    }
}
