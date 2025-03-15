using Application.Handlers.ProjectsHandler.Query;
using AutoMapper;
using Domain.Dtos.Project;
using Domain.Interfaces;
using Domain.Models;
using Infrastructure.Factory;
using MediatR;

namespace Application.Handlers.ProjectsHandler
{
    public class GetProjectHandler(IProjectService projectService, KubernetesServiceFactory serviceFactory, IMapper mapper)
        : IRequestHandler<GetProjectQuery, ProjectDto>, IRequestHandler<GetAllProjectsQuery, Pagination<ProjectDto>>
    {
        private readonly IProjectService _projectService = projectService;
        private readonly KubernetesServiceFactory _serviceFactory = serviceFactory;
        private readonly IMapper _mapper = mapper;

        public async Task<ProjectDto> Handle(GetProjectQuery request, CancellationToken cancellationToken)
        {
            var project = await _projectService.Get(request.Id);

            foreach (var service in project.Services)
            {
                var handler = _serviceFactory.GetServiceHandler(service.ServiceType);

                var serviceDto = _mapper.Map<KubernetesServiceDto>(service);

                var status = await handler.GetServiceStatusAsync(serviceDto);
                var metrics = await handler.GetServiceMetricsAsync(serviceDto);

                project.ContainerMetrics.Usage.Cpu += metrics.Usage.Cpu;
                project.ContainerMetrics.Usage.Memory += metrics.Usage.Memory;
                project.ContainerMetrics.UpTime += metrics.UpTime;

                service.ContainerStatusType = status;
                service.ContainerMetrics = metrics;
            }

            return project;
        }

        public async Task<Pagination<ProjectDto>> Handle(GetAllProjectsQuery request, CancellationToken cancellationToken)
        {
            var projects = await _projectService.Get(request.PageIndex, request.PageSize);

            foreach (var project in projects.Items)
            {
                foreach (var service in project.Services)
                {
                    var handler = _serviceFactory.GetServiceHandler(service.ServiceType);

                    var serviceDto = _mapper.Map<KubernetesServiceDto>(service);

                    var status = await handler.GetServiceStatusAsync(serviceDto);
                    var metrics = await handler.GetServiceMetricsAsync(serviceDto);

                    project.ContainerMetrics.Usage.Cpu += service.ContainerMetrics.Usage.Cpu;
                    project.ContainerMetrics.Usage.Memory += service.ContainerMetrics.Usage.Memory;
                    project.ContainerMetrics.UpTime += metrics.UpTime;

                    service.ContainerStatusType = status;
                    service.ContainerMetrics = metrics;
                }
            }

            return projects;
        }
    }
}
