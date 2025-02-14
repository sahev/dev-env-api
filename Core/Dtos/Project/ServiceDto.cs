using Core.Dtos.Kubernetes;
using Core.Entities;
using Core.Enums;
using System.Text.Json.Serialization;

namespace Core.Dtos.Project
{
    public class ServiceDto
    {
        public string Id { get; set; }
        public string Name { get; set; }
        public string? Description { get; set; }
        public string StorageSize { get; set; }
        public ServiceType ServiceType { get; set; }
        public string UserId { get; set; }
        public string ProjectId { get; set; }
        public string Host { get; set; }
        public string? User { get; set; }
        public string? Password { get; set; }
        public ContainerStatusType? ContainerStatusType { get; set; }
        public ContainerMetrics ContainerMetrics { get; set; } = new() { Usage = new() };
        public DateTime CreatedAt { get; set; }
        public string CreatedBy { get; set; }

        [JsonIgnore]
        public string PodName { get => $"{Name.Replace(" ", "-")}-{ServiceType.ToString().ToLower()}"; }
        [JsonIgnore]
        public string ServiceName { get => $"{PodName}-service"; }
        [JsonIgnore] 
        public string Namespace { get => $"{UserId}-namespace"; }
        [JsonIgnore]
        public string IngressName { get => $"{PodName}-ingress"; }
        [JsonIgnore]
        public string PvcName { get => $"{PodName}-pvc"; }
        [JsonIgnore]
        public string VolumeName { get => $"{Namespace}-storage-volume"; }

    }

    public class AddServiceRequest : BaseDto
    {
        public string Name { get; set; }
        public string? Description { get; set; }
        public string StorageSize { get; set; }
        public ServiceType ServiceType { get; set; }
        public string UserId { get; set; }
        public string ProjectId { get; set; }
        public string? Host { get; set; }
        public string? User { get; set; }
        public string? Password { get; set; }
    }

    public class UpdateServiceRequest : BaseDto
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
    }
}
