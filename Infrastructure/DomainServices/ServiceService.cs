using AutoMapper;
using Domain.Dtos.Project;
using Domain.Entities;
using Domain.Interfaces;
using Domain.Models;
using Domain.Repositories;

namespace Infrastructure.DomainServices;

public class ServiceService(IUnitOfWork unitOfWork, IMapper mapper) : IServiceService
{
    private readonly IUnitOfWork _unitOfWork = unitOfWork;
    private readonly IMapper _mapper = mapper;

    public async Task<Pagination<KubernetesServiceDto>> Get(int pageIndex, int pageSize)
    {
        var services = await _unitOfWork.ServiceRepository.ToPagination(
            pageIndex: pageIndex,
            pageSize: pageSize,
            orderBy: x => x.CreatedAt,
            ascending: true,
            selector: x => new KubernetesServiceDto
            {
                Id = x.Id,
                ProjectId = x.ProjectId,
                Name = x.Name,
                UserId = x.UserId,
                ServiceType = x.ServiceType,
                StorageSize = x.StorageSize,
                Host = x.Host,
                User = x.User,
                Password = x.Password,
                CreatedAt = x.CreatedAt,
                CreatedBy = x.CreatedBy
            }
        );

        return services;
    }

    public async Task<KubernetesServiceDto> Get(string id)
    {
        var service = await _unitOfWork.ServiceRepository.FirstOrDefaultAsync(x => x.Id == id);
        return _mapper.Map<KubernetesServiceDto>(service);
    }

    public async Task<KubernetesServiceDto> Add(AddKubernetesServiceDto request, CancellationToken token)
    {
        var service = _mapper.Map<Service>(request);
        await _unitOfWork.ExecuteTransactionAsync(async () => await _unitOfWork.ServiceRepository.AddAsync(service), token);
        return _mapper.Map<KubernetesServiceDto>(service);
    }

    public async Task<KubernetesServiceDto> Update(UpdateKubernetesServiceDto request, CancellationToken token)
    {
        if (!await _unitOfWork.ServiceRepository.AnyAsync(x => x.Id == request.Id))
            throw new UserFriendlyException("service not found", "service not found");

        var service = _mapper.Map<Service>(request);
        await _unitOfWork.ExecuteTransactionAsync(() => _unitOfWork.ServiceRepository.Update(service), token);
        return _mapper.Map<KubernetesServiceDto>(service);
    }

    public async Task<KubernetesServiceDto> Delete(string id, CancellationToken token)
    {

        var existservice = await _unitOfWork.ServiceRepository.FirstOrDefaultAsync(x => x.Id == id)
            ?? throw new UserFriendlyException("service not found", "service not found");

        await _unitOfWork.ExecuteTransactionAsync(() => _unitOfWork.ServiceRepository.Delete(existservice), token);
        return _mapper.Map<KubernetesServiceDto>(existservice);
    }
}
