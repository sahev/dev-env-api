using Application.Extensions;
using Domain.Exceptions;
using Domain.Settings;
using Infrastructure.Extensions;

var builder = WebApplication.CreateBuilder(args);

var configuration = builder.Configuration.Get<AppSettings>()
    ?? throw ProgramException.AppsettingNotSetException();

builder.Services
    .AddSingleton(configuration)
    .AddApplicationServices();

var app = await builder.ConfigureServices(configuration).ConfigurePipelineAsync(configuration);

await app.RunMigrations();

await app.RunAsync();
