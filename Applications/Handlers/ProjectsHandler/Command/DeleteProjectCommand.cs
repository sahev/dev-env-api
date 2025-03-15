using MediatR;

namespace Application.Handlers.ProjectsHandler.Command
{
    public record DeleteProjectCommand : IRequest
    {
        public string Id { get; set; }

        public static implicit operator DeleteProjectCommand(string id)
            => new() { Id = id };
    }
}
