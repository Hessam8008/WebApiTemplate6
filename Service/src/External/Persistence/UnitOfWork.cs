using Domain.Abstractions;

namespace Persistence;

public sealed class UnitOfWork : IUnitOfWork
{
    private readonly ApplicationDbContext _applicationDbContext;

    public UnitOfWork(ApplicationDbContext applicationDbContext, IPersonRepository personRepository)
    {
        _applicationDbContext = applicationDbContext;
        PersonRepository = personRepository;
    }

    public IPersonRepository PersonRepository { get; }

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