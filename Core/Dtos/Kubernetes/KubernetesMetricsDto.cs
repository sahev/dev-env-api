namespace Core.Dtos.Kubernetes
{
    public class KubernetesMetricsDto
    {
        public List<ContainerMetrics> Containers { get; set; } = new();
    }

    public class ContainerMetrics
    {
        public ResourceUsage Usage { get; set; }
        public double UpTime { get; set; }
    }

    public class ResourceUsage
    {
        public double Cpu { get; set; }
        public double Memory { get; set; }
    }
}
