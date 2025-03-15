using Application.Handlers.ProjectsHandler.Query;
using Domain.Dtos.Project;
using Domain.Interfaces;
using Domain.Models;
using Infrastructure.Factory;
using MediatR;

namespace Application.Handlers.ProjectsHandler
{
    public class GetKubernetesServiceHandler(IServiceService serviceService, KubernetesServiceFactory serviceFactory)
        : IRequestHandler<GetKubernetesServiceQuery, KubernetesServiceDto>, IRequestHandler<GetAllKubernetesServiceQuery, Pagination<KubernetesServiceDto>>
    {
        private readonly IServiceService _serviceService = serviceService;
        private readonly KubernetesServiceFactory _serviceFactory = serviceFactory;

        public async Task<KubernetesServiceDto> Handle(GetKubernetesServiceQuery request, CancellationToken cancellationToken)
        {
            var serviceDto = await _serviceService.Get(request.Id);

            var handler = _serviceFactory.GetServiceHandler(serviceDto.ServiceType);

            var status = await handler.GetServiceStatusAsync(serviceDto);
            var metrics = await handler.GetServiceMetricsAsync(serviceDto);

            serviceDto.ContainerStatusType = status;
            serviceDto.ContainerMetrics = metrics;

            return serviceDto;
        }

        public async Task<Pagination<KubernetesServiceDto>> Handle(GetAllKubernetesServiceQuery request, CancellationToken cancellationToken)
        {
            var servicesDto = await _serviceService.Get(request.PageIndex, request.PageSize);

            foreach (var serviceDto in servicesDto.Items)
            {
                var handler = _serviceFactory.GetServiceHandler(serviceDto.ServiceType);

                var status = await handler.GetServiceStatusAsync(serviceDto);
                var metrics = await handler.GetServiceMetricsAsync(serviceDto);

                serviceDto.ContainerStatusType = status;
                serviceDto.ContainerMetrics = metrics;
            }

            return servicesDto;
        }
    }
}
