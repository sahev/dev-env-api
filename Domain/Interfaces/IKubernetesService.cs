using Domain.Dtos.Kubernetes;
using Domain.Dtos.Project;
using Domain.Dtos.Services;
using Domain.Enums;

namespace Domain.Interfaces
{
    public interface IKubernetesService
    {
        Task<ServiceResponseDto> AddServiceToUserPodAsync(KubernetesServiceDto service, ProjectDto project);
        Task DeleteServiceAsync(KubernetesServiceDto service);
        Task<ContainerStatusType> GetServiceStatusAsync(KubernetesServiceDto service);
        Task<ContainerMetrics> GetServiceMetricsAsync(KubernetesServiceDto service);
    }
}
