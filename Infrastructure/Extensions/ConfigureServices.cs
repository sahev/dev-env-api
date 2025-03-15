using Domain.Settings;
using Infrastructure.Middlewares;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.DependencyInjection;
using System.Diagnostics;
using System.Text.Json;
using System.Text.Json.Serialization;

namespace Infrastructure.Extensions;

public static class ConfigureServices
{
    public static IServiceCollection AddWebAPIService(this IServiceCollection services, AppSettings appSettings)
    {
        services.AddControllers();
        services.AddEndpointsApiExplorer();
        services.AddDistributedMemoryCache();
        services.AddMemoryCache();

        // Middleware
        services.AddSingleton<GlobalExceptionMiddleware>();
        services.AddSingleton<PerformanceMiddleware>();
        services.AddSingleton<Stopwatch>();

        // Extension classes
        services.AddHealthChecks();
        services.AddHttpClient();
        services.AddSwaggerOpenAPI(appSettings);
        services.AddSingleton<IHttpContextAccessor, HttpContextAccessor>();

        services
            .AddKubernetesServices()
            .AddServices();

        // Json configuration
        services.ConfigureHttpJsonOptions(options =>
        {
            options.SerializerOptions.WriteIndented = true;
            options.SerializerOptions.DefaultIgnoreCondition = JsonIgnoreCondition.WhenWritingNull;
            options.SerializerOptions.PropertyNamingPolicy = JsonNamingPolicy.CamelCase;
            options.SerializerOptions.Converters.Add(new JsonStringEnumConverter());
        });
        return services;
    }
}
