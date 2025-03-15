using Domain.Interfaces;
using Infrastructure.DomainServices;
using Infrastructure.Factory;
using Infrastructure.KubernetesServices;
using Microsoft.Extensions.DependencyInjection;

namespace Infrastructure.Extensions
{
    public static class ServiceExtensions
    {
        public static IServiceCollection AddServices(this IServiceCollection services)
        {
            return services
                .AddScoped<KubernetesServiceFactory>()
                .AddScoped<IProjectService, ProjectService>()
                .AddScoped<IServiceService, ServiceService>()
                .AddScoped<IKubernetesService, RedisService>();
        }
    }
}
