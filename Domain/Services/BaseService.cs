using Core.Dtos.Project;
using Core.Enums;

namespace Domain.Services
{
    public abstract class BaseService 
    {
        private readonly KubernetesManager _podManager;

        public BaseService(KubernetesManager podManager)
        {
            _podManager = podManager;
        }

        public async Task DeleteServiceAsync(ServiceDto service)
        {
            await _podManager.DeletePodAsync(service);
        }

        public async Task<ContainerStatusType> GetServiceStatusAsync(ServiceDto service)
        {
            return await _podManager.GetContainerStatusAsync(service);
        }
    }
}
