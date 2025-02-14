using Core.Dtos.Kubernetes;
using Core.Entities;
using Core.Utilities;
using System.ComponentModel.DataAnnotations;

namespace Core.Dtos.Project
{
    public class ProjectDto
    {
        public string Id { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public string StorageSize { get; set; }
        public double AvailableStorageSize { get => StringHelper.GetAvailableStorageSizeInBytes(StorageSize, Services); }
        public ContainerMetrics ContainerMetrics { get; set; } = new() { Usage = new() };
        public ICollection<Service>? Services { get; set; }
        public DateTime CreatedAt { get; set; }
        public string CreatedBy { get; set; }
    }

    public class AddProjectRequest : BaseDto
    {
        [MaxLength(25)]
        public string Name { get; set; }
        public string? Description { get; set; }
        public string StorageSize { get; set; }
    }

    public class UpdateProjectRequest : BaseDto
    {
        public string Id { get; set; }
        public string Name { get; set; }
        public string? Description { get; set; }
        public string StorageSize { get; set; }
    }
}
