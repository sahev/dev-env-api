using Domain.Dtos.Project;
using Domain.Dtos.Services;
using Domain.Enums;
using Domain.Interfaces;
using Infrastructure.DomainServices;
using k8s.Models;

namespace Infrastructure.KubernetesServices
{
    public class RabbitMQService : IKubernetesService
    {
        private readonly KubernetesManager _podManager;
        private readonly string _containerName = ServiceType.RabbitMQ.ToString().ToLower();

        public RabbitMQService(KubernetesManager podManager)
        {
            _podManager = podManager;
        }

        public async Task<ServiceResponseDto> AddServiceToUserPodAsync(KubernetesServiceDto service, ProjectDto project)
        {
            string namespaceName = $"{service.UserId}-namespace";

            var container = new V1Container
            {
                Name = _containerName,
                Image = "rabbitmq:management", // Imagem com o plugin de gerenciamento habilitado
                Ports = new List<V1ContainerPort>
                {
                    new V1ContainerPort(5672), // Porta padrão do RabbitMQ
                    new V1ContainerPort(15672) // Porta do RabbitMQ Management Plugin
                },
                VolumeMounts = new List<V1VolumeMount>
                {
                    new V1VolumeMount
                    {
                        Name = $"{namespaceName}-storage-volume",
                        MountPath = "/var/lib/rabbitmq"
                    }
                },
                Env = new List<V1EnvVar>
                {
                    new V1EnvVar("RABBITMQ_DEFAULT_USER", "admin"), // Usuário padrão
                    new V1EnvVar("RABBITMQ_DEFAULT_PASS", "admin")  // Senha padrão
                },
                Resources = new V1ResourceRequirements
                {
                    Requests = new Dictionary<string, ResourceQuantity>
                    {
                        { "cpu", new ResourceQuantity("500m") },
                        { "memory", new ResourceQuantity("1Gi") }
                    },
                    Limits = new Dictionary<string, ResourceQuantity>
                    {
                        { "cpu", new ResourceQuantity("500m") },
                        { "memory", new ResourceQuantity("1Gi") }
                    }
                }
            };

            await _podManager.EnsureUserPodExistsAsync(service, container);
            await _podManager.CreateServiceAsync(service, container);
            var address = await _podManager.CreateServiceIngressAsync(service, container);

            return new ServiceResponseDto()
            {
                Host = address,
                ServiceType = ServiceType.RabbitMQ
            };
        }

        public async Task DeleteServiceAsync(KubernetesServiceDto service)
        {
            await _podManager.DeletePodAsync(service);
        }

        public async Task<ContainerStatusType> GetServiceStatusAsync(KubernetesServiceDto service)
        {
            return await _podManager.GetContainerStatusAsync(service);
        }

        public async Task<Domain.Dtos.Kubernetes.ContainerMetrics> GetServiceMetricsAsync(KubernetesServiceDto service)
        {
            return await _podManager.GetContainerMetricsAsync(service);
        }
    }
}