using Domain.Abstractions;

namespace Persistence;

public sealed class UnitOfWork : IUnitOfWork
{
    private readonly ApplicationDbContext _applicationDbContext;

    public UnitOfWork(ApplicationDbContext applicationDbContext, IContactRepository contactRepository)
    {
        _applicationDbContext = applicationDbContext;
        ContactRepository = contactRepository;
    }


    public IContactRepository ContactRepository { get; }

    public async Task<int> SaveChangesAsync(CancellationToken cancellationToken = default)
    {
        //await using var transaction = await _applicationDbContext.Database.BeginTransactionAsync(cancellationToken);

        var result = await _applicationDbContext
                .SaveChangesAsync(cancellationToken)
            ;

        //await transaction.CommitAsync(cancellationToken);

        return result;
    }
}