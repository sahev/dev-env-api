using Core.Services;
using Domain.Factory.Services;
using Domain.Services;
using Microsoft.Extensions.DependencyInjection;

namespace Infrastructure.Extensions
{
    public static class ServiceExtensions
    {
        public static IServiceCollection AddServices(this IServiceCollection services)
        {
            return services
                .AddScoped<ServiceFactory>()
                .AddScoped<IProjectService, ProjectService>()
                .AddScoped<IServiceService, ServiceService>();
        }
    }
}
