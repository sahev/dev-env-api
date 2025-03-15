namespace Domain.Enums
{
    public enum ContainerStatusType
    {
        Running,
        Terminated,
        Waiting,
        Unknown,
        PodNotFound,
        ContainerNotFound
    }
}
