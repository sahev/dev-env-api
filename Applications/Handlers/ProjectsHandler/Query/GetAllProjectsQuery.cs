using Domain.Dtos.Project;
using Domain.Models;
using MediatR;

namespace Application.Handlers.ProjectsHandler.Query
{
    public record GetAllProjectsQuery : IRequest<Pagination<ProjectDto>>
    {
        public int PageIndex { get; set; }
        public int PageSize { get; set; }
    }
}
