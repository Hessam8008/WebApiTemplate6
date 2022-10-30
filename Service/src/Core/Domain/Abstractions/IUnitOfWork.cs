namespace Domain.Abstractions;

public interface IUnitOfWork
{
    IContactRepository ContactRepository { get; }

    Task<int> SaveChangesAsync(CancellationToken cancellationToken = default);
}