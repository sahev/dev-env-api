using Domain.Dtos.Kubernetes;
using MediatR;

namespace Application.Handlers.KubernetesHandler.Command
{
    public record AddNamespaceCommand : IRequest<NamespaceDto>
    {
        public string UserId { get; set; }
    }
}
