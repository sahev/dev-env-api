using Core.Dtos.Services;

namespace Core.Dtos.Kubernetes
{
    public class ProjectsByNamespaceResponseDto
    {
        public string ProjectName { get; set; }
        public List<ServiceResponseDto> Services { get; set; }
    }
}
