using Infrastructure.DomainServices;
using Infrastructure.Factory;
using Infrastructure.KubernetesServices;
using k8s;
using Microsoft.Extensions.DependencyInjection;

namespace Infrastructure.Extensions;
public static class KubernetesExtensions
{
    public static IServiceCollection AddKubernetesServices(this IServiceCollection services)
        => services
        .AddScoped<Kubernetes>(a =>
            {
                string kubeConfigPath = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.UserProfile), ".kube", "config");
                var config = KubernetesClientConfiguration.BuildConfigFromConfigFile(kubeConfigPath);
                return new(config);
            })
        .AddScoped<KubernetesManager>()
        .AddScoped<PostgreSQLService>()
        .AddScoped<RedisService>()
        .AddScoped<RabbitMQService>()
        .AddScoped<KafkaService>()
        .AddScoped<KubernetesServiceFactory>();
}
