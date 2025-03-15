using MediatR;

namespace Application.Handlers.ProjectsHandler.Command
{
    public record DeleteKubernetesServiceCommand : IRequest
    {
        public string Id { get; set; }

        public static implicit operator DeleteKubernetesServiceCommand(string id)
            => new() { Id = id };
    }
}
