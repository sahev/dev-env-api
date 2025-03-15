using Domain.Dtos.Project;
using Domain.Dtos.Services;
using Domain.Enums;
using Domain.Interfaces;
using Infrastructure.DomainServices;
using k8s.Models;

namespace Infrastructure.KubernetesServices
{
    public class KafkaService : IKubernetesService
    {
        private readonly KubernetesManager _podManager;
        private readonly string _containerName = ServiceType.Kafka.ToString().ToLower();

        public KafkaService(KubernetesManager podManager)
        {
            _podManager = podManager;
        }

        public async Task<ServiceResponseDto> AddServiceToUserPodAsync(KubernetesServiceDto service, ProjectDto project)
        {
            string namespaceName = $"{service.UserId}-namespace";

            var container = new V1Container
            {
                Name = _containerName,
                Image = "bitnami/kafka:latest", // Imagem oficial do Kafka da Bitnami
                Ports = new List<V1ContainerPort>
                {
                    new V1ContainerPort(9092) // Porta padrão do Kafka
                },
                VolumeMounts = new List<V1VolumeMount>
                {
                    new V1VolumeMount
                    {
                        Name = $"{namespaceName}-storage-volume",
                        MountPath = "/bitnami/kafka" // Caminho padrão para dados do Kafka
                    }
                },
                Env = new List<V1EnvVar>
                {
                    new V1EnvVar("KAFKA_CFG_LISTENERS", "PLAINTEXT://:9092"),
                    new V1EnvVar("KAFKA_CFG_ADVERTISED_LISTENERS", "PLAINTEXT://kafka:9092"),
                    new V1EnvVar("KAFKA_CFG_ZOOKEEPER_CONNECT", "zookeeper:2181") // Kafka depende do Zookeeper
                },
                Resources = new V1ResourceRequirements
                {
                    Requests = new Dictionary<string, ResourceQuantity>
                    {
                        { "cpu", new ResourceQuantity("1000m") }, // Kafka pode exigir mais CPU
                        { "memory", new ResourceQuantity("2Gi") }  // Kafka pode exigir mais memória
                    },
                    Limits = new Dictionary<string, ResourceQuantity>
                    {
                        { "cpu", new ResourceQuantity("2000m") },
                        { "memory", new ResourceQuantity("4Gi") }
                    }
                }
            };

            await _podManager.EnsureUserPodExistsAsync(service, container);
            await _podManager.CreateServiceAsync(service, container);
            var address = await _podManager.CreateServiceIngressAsync(service, container);

            return new ServiceResponseDto()
            {
                Host = address,
                ServiceType = ServiceType.Kafka
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