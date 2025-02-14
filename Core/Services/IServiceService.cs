using Core.Dtos.Project;
using Core.Models;

namespace Core.Services;

public interface IServiceService
{
    Task<Pagination<ServiceDto>> Get(int pageIndex, int pageSize);
    Task<ServiceDto> Get(string id);
    Task<ServiceDto> Add(AddServiceRequest request, CancellationToken token);
    Task<ServiceDto> Update(UpdateServiceRequest request, CancellationToken token);
    Task<ServiceDto> Delete(string id, CancellationToken token);
}
