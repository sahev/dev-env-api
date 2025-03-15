using Domain.Dtos.Project;
using Domain.Models;

namespace Domain.Interfaces;

public interface IServiceService
{
    Task<Pagination<KubernetesServiceDto>> Get(int pageIndex, int pageSize);
    Task<KubernetesServiceDto> Get(string id);
    Task<KubernetesServiceDto> Add(AddKubernetesServiceDto request, CancellationToken token);
    Task<KubernetesServiceDto> Update(UpdateKubernetesServiceDto request, CancellationToken token);
    Task<KubernetesServiceDto> Delete(string id, CancellationToken token);
}
