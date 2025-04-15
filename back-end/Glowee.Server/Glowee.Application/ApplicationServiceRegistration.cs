using Glowee.Application.Contracts.Mappers;
using Glowee.Application.MappingProfiles;
using Microsoft.Extensions.DependencyInjection;
using System.Reflection;

namespace Glowee.Application
{
    public static class ApplicationServiceRegistration
    {
        public static IServiceCollection AddApplicationServices(this IServiceCollection services)
        {
            services.AddMediatR(config => config.RegisterServicesFromAssembly(Assembly.GetExecutingAssembly()));

            services.AddScoped<IPostMapper, PostMapper>();
            services.AddScoped<ICommentMapper, CommentMapper>();

            return services;
        }
    }
}
