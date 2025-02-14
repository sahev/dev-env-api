using Core.Constants;
using Core.Settings;
using HealthChecks.UI.Client;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Diagnostics.HealthChecks;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.DependencyInjection;

namespace Infrastructure.Extensions;
/// <summary>
/// Provides extension methods for configuring health checks
/// </summary>
public static class HealthCheckExtensions
{
    public static void SetupHealthCheck(this IServiceCollection services, AppSettings configuration)
    {
        // Add health checks
        var healthCheckBuilder = services.AddHealthChecks();

        // Configure Health Check UI
        services.AddHealthChecksUI(setup =>
            setup.AddHealthCheckEndpoint(
                "Application Health", configuration.AppUrl + "healthz"));
    }

    /// <summary>
    /// Configures health checks for the application
    /// </summary>
    /// <param name="app"></param>
    public static void ConfigureHealthCheck(this WebApplication app)
    {
        // Health check endpoint (basic and detailed combined)
        app.UseHealthChecks("/health", new HealthCheckOptions
        {
            Predicate = _ => true,
            ResponseWriter = async (context, report) =>
            {
                context.Response.ContentType = "application/json";

                var response = new
                {
                    status = report.Status.ToString(),
                    checks = report.Entries.Select(entry => new
                    {
                        name = entry.Key,
                        status = entry.Value.Status.ToString(),
                        description = entry.Value.Description,
                        duration = entry.Value.Duration
                    }),
                    totalDuration = report.TotalDuration
                };
                await context.Response.WriteAsJsonAsync(response);
            }
        });

        app.UseHealthChecks("/synthetic-check", new HealthCheckOptions
        {
            Predicate = check =>
            check.Tags.Contains(HealthCheck.InfrastructureCheck) ||
            check.Tags.Contains(HealthCheck.ExternalServiceCheck)
        });

        app.MapHealthChecks("/healthz", new HealthCheckOptions
        {
            Predicate = _ => true,
            ResponseWriter = UIResponseWriter.WriteHealthCheckUIResponse
        });
    }
}
