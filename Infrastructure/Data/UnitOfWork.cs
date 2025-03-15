using Domain.Repositories;
using Infrastructure.Data.Repositories;

namespace Infrastructure.Data;

public class UnitOfWork : IUnitOfWork
{
    private readonly ApplicationDbContext _context;

    public IUserRepository UserRepository { get; }
    public IProjectRepository ProjectRepository { get; }
    public IServiceRepository ServiceRepository { get; }

    public UnitOfWork(ApplicationDbContext dbContext)
    {
        _context = dbContext;
        UserRepository = new UserRepository(_context);
        ProjectRepository = new ProjectRepository(_context);
        ServiceRepository = new ServiceRepository(_context);
    }

    public async Task SaveChangesAsync(CancellationToken token)
        => await _context.SaveChangesAsync(token);

    public async Task ExecuteTransactionAsync(Action action, CancellationToken token)
    {
        using var transaction = await _context.Database.BeginTransactionAsync(token);
        try
        {
            action();
            await _context.SaveChangesAsync(token);
            await transaction.CommitAsync(token);
        }
        catch (Exception ex)
        {
            await transaction.RollbackAsync(token);
            throw TransactionException.TransactionNotExecuteException(ex);
        }
    }

    public async Task ExecuteTransactionAsync(Func<Task> action, CancellationToken token)
    {
        using var transaction = await _context.Database.BeginTransactionAsync(token);
        try
        {
            await action();
            await _context.SaveChangesAsync(token);
            await transaction.CommitAsync(token);
        }
        catch (Exception ex)
        {
            await transaction.RollbackAsync(token);
            throw TransactionException.TransactionNotExecuteException(ex);
        }
    }
}
