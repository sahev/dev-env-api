using Core.Repositories;
using Core.Settings;
using Infrastructure.Data.Repositories;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Diagnostics;
using Microsoft.Extensions.DependencyInjection;

namespace Infrastructure.Data;

public static class ConfigureServices
{
    public static IServiceCollection AddInfrastructuresService(this IServiceCollection services, AppSettings configuration)
    {

        services.AddDbContext<ApplicationDbContext>(options =>
        {
            if (configuration.UseInMemoryDatabase)
            {
                options.UseInMemoryDatabase("DevEnv")
                    .ConfigureWarnings(warnings => warnings.Ignore(InMemoryEventId.TransactionIgnoredWarning));
            }
            else
            {
                options.UseNpgsql(configuration.ConnectionStrings.DefaultConnection);
            }
        });

        // register services
        services.AddScoped<IUserRepository, UserRepository>();
        services.AddScoped<IProjectRepository, ProjectRepository>();
        services.AddScoped<IServiceRepository, ServiceRepository>();
        services.AddScoped<IUnitOfWork, UnitOfWork>();

        services.AddCors(options => options.AddPolicy("CorsPolicy",
                builder =>
                {
                    builder.AllowAnyHeader()
                           .AllowAnyMethod()
                           .SetIsOriginAllowed((host) => true)
                           .AllowCredentials();
                }));

        return services;
    }
}
