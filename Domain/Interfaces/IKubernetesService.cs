using Core.Dtos.Kubernetes;
using Core.Dtos.Project;
using Core.Dtos.Services;
using Core.Enums;

namespace Domain.Interfaces
{
    public interface IKubernetesService
    {
        Task<ServiceResponseDto> AddServiceToUserPodAsync(ServiceDto service, ProjectDto project);
        Task DeleteServiceAsync(ServiceDto service);
        Task<ContainerStatusType> GetServiceStatusAsync(ServiceDto service);
        Task<ContainerMetrics> GetServiceMetricsAsync(ServiceDto service);
    }
}
