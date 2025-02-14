using Core.Enums;
using Domain.Interfaces;
using Domain.Services;

namespace Domain.Factory.Services
{
    public class ServiceFactory
    {
        private readonly KubernetesManager _podManager;

        public ServiceFactory(KubernetesManager podManager)
        {
            _podManager = podManager;
        }

        public IKubernetesService GetServiceHandler(ServiceType serviceType)
        {
            return serviceType switch
            {
                ServiceType.Postgres => new PostgreSQLService(_podManager),
                ServiceType.Redis => new RedisService(_podManager),
                _ => throw new ArgumentException("Service type not supported")
            };
        }
    }
}
