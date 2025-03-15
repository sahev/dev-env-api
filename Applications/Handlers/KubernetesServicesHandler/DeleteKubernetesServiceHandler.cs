using Application.Handlers.ProjectsHandler.Command;
using Domain.Interfaces;
using Infrastructure.Factory;
using MediatR;

namespace Application.Handlers.KubernetesHandler
{
    public class DeleteKubernetesServiceHandler(KubernetesServiceFactory serviceFactory, IServiceService serviceService)
        : IRequestHandler<DeleteKubernetesServiceCommand>
    {
        private readonly KubernetesServiceFactory _serviceFactory = serviceFactory;
        private readonly IServiceService _serviceService = serviceService;

        public async Task Handle(DeleteKubernetesServiceCommand request, CancellationToken cancellationToken)
        {

            var service = await _serviceService.Delete(request.Id, cancellationToken);

            var handler = _serviceFactory.GetServiceHandler(service.ServiceType);

            await handler.DeleteServiceAsync(service);

            return;
        }
    }
}
