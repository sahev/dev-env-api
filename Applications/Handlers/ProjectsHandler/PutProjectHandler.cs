using Application.Handlers.ProjectsHandler.Command;
using AutoMapper;
using Domain.Dtos.Project;
using Domain.Interfaces;
using MediatR;

namespace Application.Handlers.KubernetesHandler
{
    public class PutProjectHandler(IProjectService projectService, IMapper mapper) : IRequestHandler<PutProjectCommand, ProjectDto>
    {
        private readonly IProjectService _projectService = projectService;
        private readonly IMapper _mapper = mapper;

        public async Task<ProjectDto> Handle(PutProjectCommand request, CancellationToken cancellationToken)
        {
            var mapRequest = _mapper.Map<UpdateProjectDto>(request);

            return await _projectService.Update(mapRequest, cancellationToken);
        }
    }
}
