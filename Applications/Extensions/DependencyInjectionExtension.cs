using Application.Handlers.ProjectsHandler;
using Domain.Mapping;
using Microsoft.Extensions.DependencyInjection;

namespace Application.Extensions
{
    public static class DependencyInjectionExtension
    {
        public static IServiceCollection AddApplicationServices(this IServiceCollection services)
            => services
            .AddAutoMapper(typeof(MapProfile))
            .AddMediatR(cfg => cfg.RegisterServicesFromAssembly(typeof(GetProjectHandler).Assembly));
    }
}
