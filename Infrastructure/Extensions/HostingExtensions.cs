using Domain.Settings;
using Infrastructure.Data;
using Infrastructure.Middlewares;
using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;

namespace Infrastructure.Extensions;

public static class HostingExtensions
{
    public static WebApplication ConfigureServices(this WebApplicationBuilder builder, AppSettings appsettings)
    {
        builder.Services
            .AddWebAPIService(appsettings)
            .AddInfrastructuresService(appsettings);

        return builder.Build();
    }

    public static async Task<WebApplication> ConfigurePipelineAsync(this WebApplication app, AppSettings appsettings)
    {
        using var loggerFactory = LoggerFactory.Create(builder => { });
        using var scope = app.Services.CreateScope();

        app.UseMiddleware<GlobalExceptionMiddleware>();
        app.ConfigureExceptionHandler(loggerFactory.CreateLogger("Exceptions"));
        app.UseMiddleware<PerformanceMiddleware>();
        app.UseHttpsRedirection();
        app.UseCors("CorsPolicy");
        app.UseSwagger(appsettings);
        app.ConfigureHealthCheck();
        app.UseAuthentication();
        app.UseAuthorization();
        app.MapControllers();

        return app;
    }

}
