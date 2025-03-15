using Application.Handlers.Common;
using Domain.Dtos.Project;
using MediatR;

namespace Application.Handlers.ProjectsHandler.Command
{
    public record PutProjectCommand : BaseCommand, IRequest<ProjectDto>
    {
        public string Id { get; set; }
        public string Name { get; set; }
        public string? Description { get; set; }
        public string StorageSize { get; set; }
    }
}
