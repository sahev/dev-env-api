using Domain.Enums;

namespace Domain.Dtos.Services
{
    public class ServiceResponseDto
    {
        public ServiceType ServiceType { get; set; }
        public string User { get; set; }
        public string Password { get; set; }
        public string Host { get; set; }
        public ServiceStatus ServiceStatus { get; set; }
    }
}
