namespace Core.Repositories;

public interface IUnitOfWork
{
    IUserRepository UserRepository { get; }
    IProjectRepository ProjectRepository { get; }
    IServiceRepository ServiceRepository { get; }
    Task SaveChangesAsync(CancellationToken token);
    Task ExecuteTransactionAsync(Action action, CancellationToken token);
    Task ExecuteTransactionAsync(Func<Task> action, CancellationToken token);
}
