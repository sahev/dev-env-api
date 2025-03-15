using Domain.Enums;

namespace Domain.Dtos.Services
{
    public class DeleteServiceDto
    {
        public ServiceType ServiceType { get; set; }
        public string UserId { get; set; }
        public string ProjectId { get; set; }

    }
}
