using Livemedy.Domain.Repositories.Base;
using Livemedy.Infrastructure.Repositories.Base;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace Livemedy.Infrastructure;

public static class DependencyInjection
{
    public static IServiceCollection AddInfrastructure(this IServiceCollection services, IConfiguration configuration)
    {
        services.AddDbContext<ApplicationContext>(options =>
                options.UseSqlServer(
                    configuration.GetConnectionString("DefaultConnectionString"),
                    conf =>
                    {
                        conf.MigrationsAssembly(typeof(ApplicationContext).Assembly.FullName);
                        //conf.UseHierarchyId();
                    }), ServiceLifetime.Transient);

        //Add Repositories
        services.AddTransient<IUnitOfWork, UnitOfWork>();

        return services;
    }
}
