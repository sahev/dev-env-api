using Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using System.Reflection;

namespace Infrastructure.Data;

public class ApplicationDbContext : DbContext
{
    public DbSet<Service> Services { get; set; }
    public DbSet<User> Users { get; set; }
    public DbSet<Project> Projects { get; set; }

    public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
        : base(options)
    {
    }

    protected override void OnModelCreating(ModelBuilder builder)
    {
        base.OnModelCreating(builder);
        builder.ApplyConfigurationsFromAssembly(Assembly.GetExecutingAssembly());
    }

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
    {
        if (!optionsBuilder.IsConfigured)
        {
            var env = Environment.GetEnvironmentVariable("ASPNETCORE_ENVIRONMENT");

            if (string.IsNullOrEmpty(env))
                env = "Development";

            var configuration = new ConfigurationBuilder()
                .AddJsonFile($"appsettings.{env}.json", false)
                .Build();

            var connectionString = configuration.GetConnectionString("DefaultConnection");

            var assemblyName = Assembly.GetExecutingAssembly().GetName().Name;

            optionsBuilder.UseNpgsql(connectionString, b => b.MigrationsAssembly(assemblyName));

            base.OnConfiguring(optionsBuilder);
        }
    }
}
