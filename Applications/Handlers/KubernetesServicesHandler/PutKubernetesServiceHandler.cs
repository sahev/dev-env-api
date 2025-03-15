using Application.Handlers.ProjectsHandler.Command;
using AutoMapper;
using Domain.Dtos.Project;
using Domain.Interfaces;
using MediatR;

namespace Application.Handlers.KubernetesHandler
{
    public class PutKubernetesServiceHandler(IServiceService serviceService, IMapper mapper) : IRequestHandler<PutKubernetesServiceCommand, KubernetesServiceDto>
    {
        private readonly IServiceService _serviceService = serviceService;
        private readonly IMapper _mapper = mapper;

        public async Task<KubernetesServiceDto> Handle(PutKubernetesServiceCommand request, CancellationToken cancellationToken)
        {
            var mapRequest = _mapper.Map<UpdateKubernetesServiceDto>(request);

            return await _serviceService.Update(mapRequest, cancellationToken);
        }
    }
}
