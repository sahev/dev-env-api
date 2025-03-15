using Application.Handlers.Common;
using Domain.Dtos.Project;
using MediatR;
using System.ComponentModel.DataAnnotations;

namespace Application.Handlers.ProjectsHandler.Command
{
    public record AddProjectCommand : BaseCommand, IRequest<ProjectDto>
    {
        [MaxLength(25)]
        public string Name { get; set; }
        public string? Description { get; set; }
        public string StorageSize { get; set; }
    }
}
