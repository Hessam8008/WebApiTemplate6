namespace Domain.Abstractions;

public interface IUnitOfWork
{
    IPersonRepository PersonRepository { get; }

    Task<int> SaveChangesAsync(CancellationToken cancellationToken = default);
}