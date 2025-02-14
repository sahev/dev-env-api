using Domain.Services;
using k8s;
using Microsoft.Extensions.DependencyInjection;

namespace Infrastructure.Extensions;
public static class KubernetesExtensions
{
    public static IServiceCollection AddKubernetesServices(this IServiceCollection services)
    {
        return services
            .AddScoped<Kubernetes>(a =>
            {
                string kubeConfigPath = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.UserProfile), ".kube", "config");
                var config = KubernetesClientConfiguration.BuildConfigFromConfigFile(kubeConfigPath);
                return new(config);
            })
            .AddScoped<KubernetesManager>();
    }
}
