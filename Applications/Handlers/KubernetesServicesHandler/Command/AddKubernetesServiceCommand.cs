using Application.Handlers.Common;
using Domain.Dtos.Project;
using Domain.Enums;
using MediatR;

namespace Application.Handlers.ProjectsHandler.Command
{
    public record AddKubernetesServiceCommand : BaseCommand, IRequest<KubernetesServiceDto>
    {
        public string Name { get; set; }
        public string? Description { get; set; }
        public string StorageSize { get; set; }
        public ServiceType ServiceType { get; set; }
        public string UserId { get; set; }
        public string ProjectId { get; set; }
        public string? Host { get; set; }
        public string? User { get; set; }
        public string? Password { get; set; }
    }
}
