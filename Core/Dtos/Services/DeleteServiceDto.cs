using Core.Enums;

namespace Core.Dtos.Services
{
    public class DeleteServiceDto
    {
        public ServiceType ServiceType { get; set; }
        public string UserId { get; set; }
        public string ProjectId { get; set; }

    }
}
