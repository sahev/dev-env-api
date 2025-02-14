namespace Core.Dtos.Kubernetes
{
    public class NamespaceDto(string name)
    {
        public string Namespace { get; set; } = name;
    }
}
