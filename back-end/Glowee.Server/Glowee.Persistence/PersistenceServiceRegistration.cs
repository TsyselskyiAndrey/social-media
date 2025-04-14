using Glowee.Application.Contracts.Persistence;
using Glowee.Persistence.DbContext;
using Glowee.Persistence.Repositories;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace Glowee.Persistence
{
    public static class PersistenceServiceRegistration
    {
        public static IServiceCollection AddPersistenceServices(this IServiceCollection services,
        IConfiguration configuration)
        {
            var connectionString = configuration.GetConnectionString("DefaultConnection")
                                           ?? throw new InvalidOperationException("Connection string 'DefaultConnection' not found.");

            services.AddDbContext<SqlDbContext>(options => options.UseSqlServer(connectionString));
            services.AddScoped(typeof(IGenericRepository<,>), typeof(GenericRepository<,>));
            services.AddScoped<IUserRepository, UserRepository>();

            return services;
        }
    }
}
