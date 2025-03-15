using Domain.Enums;
using Domain.Interfaces;
using Infrastructure.KubernetesServices;
using Microsoft.Extensions.DependencyInjection;

namespace Infrastructure.Factory
{
    public class KubernetesServiceFactory
    {
        private readonly IServiceProvider _serviceProvider;

        public KubernetesServiceFactory(IServiceProvider serviceProvider)
        {
            _serviceProvider = serviceProvider;
        }

        public IKubernetesService GetServiceHandler(ServiceType serviceType)
        {
            return serviceType switch
            {
                ServiceType.Postgres => _serviceProvider.GetRequiredService<PostgreSQLService>(),
                ServiceType.Redis => _serviceProvider.GetRequiredService<RedisService>(),
                ServiceType.RabbitMQ => _serviceProvider.GetRequiredService<RabbitMQService>(),
                ServiceType.Kafka => _serviceProvider.GetRequiredService<KafkaService>(),
                _ => throw new ArgumentException("Service type not supported")
            };
        }
    }
}
