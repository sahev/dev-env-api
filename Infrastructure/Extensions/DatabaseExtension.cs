using Core.Entities;
using Infrastructure.Data;
using Microsoft.AspNetCore.Builder;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using System.Linq;
using System.Text.RegularExpressions;

namespace Infrastructure.Extensions;

public static class DatabaseExtension
{
    public static async Task RunMigrations(this WebApplication app)
    {
        await using var scope = app.Services.CreateAsyncScope();

        DbContext context = scope.ServiceProvider.GetService<ApplicationDbContext>() ?? throw new InvalidOperationException();

        var pendingMigrations = await context!.Database.GetPendingMigrationsAsync();
        var con = context.Database.GetConnectionString();

        try
        {
            if (pendingMigrations.Any())
            {
                Console.WriteLine($"You have {pendingMigrations.Count()} pending migrations to apply.");
                Console.WriteLine("Applying pending migrations now...");
                context.Database.Migrate();
            }

            var lastAppliedMigration = (await context.Database.GetAppliedMigrationsAsync()).Last();
            Console.WriteLine($"You're on latest schema version: {lastAppliedMigration}");
        }
        catch (Exception ex)
        {
            throw;
        }
    }
}
