using Application.Handlers.ProjectsHandler.Command;
using AutoMapper;
using Domain.Dtos.Project;
using Domain.Entities;
using Domain.Exceptions;
using Domain.Interfaces;
using Infrastructure.Factory;
using MediatR;

namespace Application.Handlers.KubernetesHandler
{
    public class DeleteProjectHandler(IProjectService projectService, KubernetesServiceFactory serviceFactory, IServiceService serviceService, IMapper mapper)
        : IRequestHandler<DeleteProjectCommand>
    {
        private readonly IProjectService _projectService = projectService;
        private readonly KubernetesServiceFactory _serviceFactory = serviceFactory;
        private readonly IServiceService _serviceService = serviceService;
        private readonly IMapper _mapper = mapper;

        public async Task Handle(DeleteProjectCommand request, CancellationToken cancellationToken)
        {
            var project = await _projectService.Get(request.Id) ?? throw new UserFriendlyException("project not found", "project not found");

            foreach (Service service in project.Services)
            {
                await _serviceService.Delete(service.Id, cancellationToken);

                var handler = _serviceFactory.GetServiceHandler(service.ServiceType);

                var serviceDto = _mapper.Map<KubernetesServiceDto>(service);

                await handler.DeleteServiceAsync(serviceDto);
            }

            await _projectService.Delete(request.Id, cancellationToken);

            return;
        }
    }
}
