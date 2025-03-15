using Domain.Dtos.Project;
using Domain.Dtos.Services;
using Domain.Enums;
using Domain.Interfaces;
using Infrastructure.DomainServices;
using k8s.Models;

namespace Infrastructure.KubernetesServices
{
    public class RedisService : IKubernetesService
    {
        private readonly KubernetesManager _podManager;
        private readonly string _containerName = ServiceType.Redis.ToString().ToLower();

        public RedisService(KubernetesManager podManager)
        {
            _podManager = podManager;
        }

        public async Task<ServiceResponseDto> AddServiceToUserPodAsync(KubernetesServiceDto service, ProjectDto project)
        {
            string namespaceName = $"{service.UserId}-namespace";

            var container = new V1Container
            {
                Name = _containerName,
                Image = "redis:latest",
                Ports = new List<V1ContainerPort> { new V1ContainerPort(6379) },
                VolumeMounts = new List<V1VolumeMount>
                {
                    new V1VolumeMount
                    {
                        Name = $"{namespaceName}-storage-volume",
                        MountPath = "/data"
                    }
                },
                Resources = new V1ResourceRequirements
                {
                    Requests = new Dictionary<string, ResourceQuantity>
                    {
                        { "cpu", new ResourceQuantity("250m") },
                        { "memory", new ResourceQuantity("500m") }
                    },
                    Limits = new Dictionary<string, ResourceQuantity>
                    {
                        { "cpu", new ResourceQuantity("250m") },
                        { "memory", new ResourceQuantity("500m") }
                    }
                }
            };

            await _podManager.EnsureUserPodExistsAsync(service, container);
            await _podManager.CreateServiceAsync(service, container);
            var address = await _podManager.CreateServiceIngressAsync(service, container);

            return new ServiceResponseDto()
            {
                Host = address,
                ServiceType = ServiceType.Redis
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
