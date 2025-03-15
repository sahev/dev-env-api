using Domain.Dtos.Project;
using MediatR;

namespace Application.Handlers.ProjectsHandler.Query
{
    public record GetProjectQuery : IRequest<ProjectDto>
    {
        public string Id { get; set; }

        public static implicit operator GetProjectQuery(string id)
            => new() { Id = id };
    }
}
