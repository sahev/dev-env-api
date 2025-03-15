using Domain.Dtos.Project;
using MediatR;

namespace Application.Handlers.ProjectsHandler.Query
{
    public record GetKubernetesServiceQuery : IRequest<KubernetesServiceDto>
    {
        public string Id { get; set; }

        public static implicit operator GetKubernetesServiceQuery(string id)
            => new() { Id = id };
    }
}
