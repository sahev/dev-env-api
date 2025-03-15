using Domain.Dtos.Project;
using Domain.Models;

namespace Domain.Interfaces;

public interface IProjectService
{
    Task<Pagination<ProjectDto>> Get(int pageIndex, int pageSize);
    Task<ProjectDto> Get(string id);
    Task<ProjectDto> Add(AddProjectDto request, CancellationToken token);
    Task<ProjectDto> Update(UpdateProjectDto request, CancellationToken token);
    Task<ProjectDto> Delete(string id, CancellationToken token);
}
