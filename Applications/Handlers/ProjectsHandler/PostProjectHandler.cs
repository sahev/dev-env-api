using Application.Handlers.ProjectsHandler.Command;
using AutoMapper;
using Domain.Dtos.Project;
using Domain.Interfaces;
using MediatR;

namespace Application.Handlers.KubernetesHandler
{
    public class PostProjectHandler(IProjectService projectService, IMapper mapper) : IRequestHandler<AddProjectCommand, ProjectDto>
    {
        private readonly IProjectService _projectService = projectService;
        private readonly IMapper _mapper = mapper;

        public async Task<ProjectDto> Handle(AddProjectCommand request, CancellationToken cancellationToken)
        {
            var mapRequest = _mapper.Map<AddProjectDto>(request);

            return await _projectService.Add(mapRequest, cancellationToken);
        }
    }
}
