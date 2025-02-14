using Core.Dtos.Kubernetes;
using Core.Enums;
using Core.Models;
using System.ComponentModel.DataAnnotations.Schema;

namespace Core.Entities
{
    public class Service : BaseModel
    {
        public string Name { get; set; }
        public string? Description { get; set; }
        public string StorageSize { get; set; }
        public ServiceType ServiceType { get; set; }
        public string UserId { get; set; }
        public string ProjectId { get; set; }
        public string Host { get; set; }
        public string? User { get; set; }
        public string? Password { get; set; }
        [NotMapped]
        public ContainerStatusType? ContainerStatusType { get; set; }
        [NotMapped]
        public ContainerMetrics ContainerMetrics { get; set; } = new() { Usage = new() };
    }
}
