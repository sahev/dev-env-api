using Domain.Settings;
using Infrastructure.Extensions;
using Infrastructure.Filters;
using MicroElements.Swashbuckle.FluentValidation.AspNetCore;
using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.OpenApi.Models;

namespace Infrastructure.Extensions;

public static class SwaggerExtension
{
    private static readonly string[] Value = ["Bearer"];

    public static IServiceCollection AddSwaggerOpenAPI(this IServiceCollection services, AppSettings appSettings)
    {
        services.AddFluentValidationRulesToSwagger();

        services.AddSwaggerGen(options =>
        {
            options.SwaggerDoc("v1", new OpenApiInfo
            {
                Title = appSettings.ApplicationDetail.ApplicationName,
                Version = "v1",
                Description = appSettings.ApplicationDetail.Description,
                Contact = new OpenApiContact
                {
                    Email = "devenv@devenv.com",
                    Name = "Samuel Evangelista",
                    Url = new Uri(appSettings.ApplicationDetail.ContactWebsite),
                },
                License = new OpenApiLicense()
                {
                    Name = "MIT License",
                    Url = new Uri("https://opensource.org/licenses/MIT")
                }
            });

            // Add security definition for Bearer token
            var securityScheme = new OpenApiSecurityScheme
            {
                Description = "JWT Authorization header using the Bearer scheme. Example: \"Authorization: Bearer {token}\"",
                Name = "Authorization",
                Scheme = "bearer",
                Reference = new OpenApiReference
                {
                    Type = ReferenceType.SecurityScheme,
                    Id = "Bearer"
                }
            };
            options.AddSecurityDefinition("Bearer", securityScheme);

            var securityRequirement = new OpenApiSecurityRequirement { { securityScheme, Value } };
            options.AddSecurityRequirement(securityRequirement);

            options.DocumentFilter<HealthChecksFilter>();
            options.EnableAnnotations();
        });
        return services;
    }

    public static void UseSwagger(this IApplicationBuilder app, AppSettings appSettings)
    {
        app.UseSwagger(c =>
        {
            c.RouteTemplate = "swagger/{documentName}/swagger.json";
            c.PreSerializeFilters.Add((swaggerDoc, httpReq)
                => swaggerDoc.Servers = [new OpenApiServer { Url = appSettings.AppUrl }]);
        });

        app.UseSwaggerUI(setupAction =>
        {
            setupAction.SwaggerEndpoint("v1/swagger.json", "DevEnv Api v1");
            setupAction.RoutePrefix = "swagger";
        });
    }
}
