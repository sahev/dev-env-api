using Domain.Dtos.Project;
using Domain.Dtos.Services;
using Domain.Enums;
using Domain.Interfaces;
using Domain.Utilities;
using Infrastructure.DomainServices;
using k8s.Models;

namespace Infrastructure.KubernetesServices
{
    public class PostgreSQLService : IKubernetesService
    {
        private readonly KubernetesManager _podManager;
        private readonly string _containerName = ServiceType.Postgres.ToString().ToLower();

        public PostgreSQLService(KubernetesManager podManager)
        {
            _podManager = podManager;
        }

        public async Task<ServiceResponseDto> AddServiceToUserPodAsync(KubernetesServiceDto service, ProjectDto project)
        {
            var password = StringHelper.GenerateRandomStringFromGuid();

            var container = new V1Container
            {
                Name = _containerName,
                Image = "postgres:latest",
                Env = new List<V1EnvVar>
                {
                    new V1EnvVar { Name = "POSTGRES_USER", Value = service.UserId},
                    new V1EnvVar { Name = "POSTGRES_PASSWORD", Value = password },
                    new V1EnvVar { Name = "POSTGRES_DB", Value = service.Name.Replace("-", "_").Replace(" ", "_") }
                },
                Ports = new List<V1ContainerPort> { new V1ContainerPort(5432) },
                VolumeMounts = new List<V1VolumeMount>
                {
                    new V1VolumeMount
                    {
                        Name = service.VolumeName,
                        MountPath = "/data"
                    }
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
                User = service.UserId,
                Password = password,
                ServiceType = ServiceType.Postgres
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
