using Domain.Dtos.Services;

namespace Domain.Dtos.Kubernetes
{
    public class ProjectsByNamespaceResponseDto
    {
        public string ProjectName { get; set; }
        public List<ServiceResponseDto> Services { get; set; }
    }
}
