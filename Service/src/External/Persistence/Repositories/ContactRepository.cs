using Domain.Abstractions;
using Domain.Entities;
using Microsoft.EntityFrameworkCore;

namespace Persistence.Repositories;

public class ContactRepository : IContactRepository
{
    private readonly ApplicationDbContext _dbContext;

    public ContactRepository(ApplicationDbContext dbContext)
    {
        _dbContext = dbContext;
    }

    public async Task InsertAsync(Contact contact, CancellationToken cancellationToken = default)
    {
        await _dbContext.Set<Contact>().AddAsync(contact, cancellationToken);
    }

    public async Task<Contact?> SelectAsync(Guid id, CancellationToken cancellationToken = default)
    {
        return await _dbContext.Set<Contact>().Where(c => c.Id.Equals(id)).FirstOrDefaultAsync(cancellationToken);
    }

    public async Task<List<Contact>> SelectAllAsync(CancellationToken cancellationToken = default)
    {
        return await _dbContext.Set<Contact>().ToListAsync(cancellationToken);
    }

    public async Task<Contact?> SelectByInternalNumberAsync(short internalNumber,
        CancellationToken cancellationToken = default)
    {
        return await _dbContext.Set<Contact>()
            .FirstOrDefaultAsync(c => c.InternalNumber.Equals(internalNumber), cancellationToken);
    }

    public async Task<bool> ExistsInternalNumber(short internalNumber, CancellationToken cancellationToken = default)
    {
        return await _dbContext.Set<Contact>()
            .AnyAsync(c => c.InternalNumber == internalNumber, cancellationToken);
    }

    public void Update(Contact contact)
    {
        _dbContext.Set<Contact>().Update(contact);
    }

    public void Delete(Contact contact)
    {
        _dbContext.Set<Contact>().Remove(contact);
    }
}