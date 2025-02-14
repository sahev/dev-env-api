using Core.Dtos.Project;
using Core.Models;

namespace Core.Services;

public interface IProjectService
{
    Task<Pagination<ProjectDto>> Get(int pageIndex, int pageSize);
    Task<ProjectDto> Get(string id);
    Task<ProjectDto> Add(AddProjectRequest request, CancellationToken token);
    Task<ProjectDto> Update(UpdateProjectRequest request, CancellationToken token);
    Task<ProjectDto> Delete(string id, CancellationToken token);
}
