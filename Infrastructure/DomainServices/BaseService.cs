using Domain.Dtos.Project;
using Domain.Enums;

namespace Infrastructure.DomainServices
{
    public abstract class BaseService
    {
        private readonly KubernetesManager _podManager;

        public BaseService(KubernetesManager podManager)
        {
            _podManager = podManager;
        }

        public async Task DeleteServiceAsync(KubernetesServiceDto service)
        {
            await _podManager.DeletePodAsync(service);
        }

        public async Task<ContainerStatusType> GetServiceStatusAsync(KubernetesServiceDto service)
        {
            return await _podManager.GetContainerStatusAsync(service);
        }
    }
}
