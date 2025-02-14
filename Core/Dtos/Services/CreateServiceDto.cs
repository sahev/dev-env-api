using Core.Enums;

namespace Core.Dtos.Services
{
    public class CreateServiceDto
    {
        public ServiceType ServiceType { get; set; }
        public string UserId { get; set; }
        public string ProjectName { get; set; }
    }
}
