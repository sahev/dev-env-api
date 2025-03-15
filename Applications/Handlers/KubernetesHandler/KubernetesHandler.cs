using Application.Handlers.KubernetesHandler.Command;
using Domain.Dtos.Kubernetes;
using Infrastructure.DomainServices;
using MediatR;

namespace Application.Handlers.KubernetesHandler
{
    public class KubernetesHandler : IRequestHandler<AddNamespaceCommand, NamespaceDto>
    {
        private readonly KubernetesManager _kubernetesManager;

        public KubernetesHandler(KubernetesManager kubernetesManager)
        {
            _kubernetesManager = kubernetesManager;
        }

        public async Task<NamespaceDto> Handle(AddNamespaceCommand request, CancellationToken cancellationToken)
        {
            return await _kubernetesManager.CreateNamespaceAsync(request.UserId);
        }
    }
}
