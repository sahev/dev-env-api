using Application.Handlers.ProjectsHandler.Command;
using AutoMapper;
using Domain.Dtos.Project;
using Domain.Interfaces;
using Infrastructure.Factory;
using MediatR;

namespace Application.Handlers.KubernetesHandler
{
    public class PostKubernetesServiceHandler(IServiceService serviceService, KubernetesServiceFactory kubernetesServiceFactory, IProjectService projectService, IMapper mapper) : IRequestHandler<AddKubernetesServiceCommand, KubernetesServiceDto>
    {
        private readonly IServiceService _serviceService = serviceService;
        private readonly KubernetesServiceFactory _kubernetesServiceFactory = kubernetesServiceFactory;
        private readonly IProjectService _projectService = projectService;
        private readonly IMapper _mapper = mapper;

        public async Task<KubernetesServiceDto> Handle(AddKubernetesServiceCommand request, CancellationToken cancellationToken)
        {
            var handler = _kubernetesServiceFactory.GetServiceHandler(request.ServiceType);

            var project = await _projectService.Get(request.ProjectId);

            var serviceDto = _mapper.Map<KubernetesServiceDto>(request);

            var kube = await handler.AddServiceToUserPodAsync(
                serviceDto, project
            );

            request.Host = kube.Host;
            request.User = kube.User;
            request.Password = kube.Password;

            var addRequest = _mapper.Map<AddKubernetesServiceDto>(request);

            var service = await _serviceService.Add(addRequest, cancellationToken);

            return service;
        }
    }
}
