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

    public async Task InsertAsync(Contact contact, CancellationToken cancellationToken)
    {
        await _dbContext.Set<Contact>().AddAsync(contact, cancellationToken);
    }

    public async Task<Contact?> SelectAsync(Guid id, CancellationToken cancellationToken)
    {
        return await _dbContext.Set<Contact>().Where(c => c.Id.Equals(id)).FirstOrDefaultAsync(cancellationToken);
    }

    public async Task<List<Contact>> SelectAllAsync(CancellationToken cancellationToken)
    {
        return await _dbContext.Set<Contact>().ToListAsync(cancellationToken);
    }

    public async Task<Contact?> SelectByInternalNumberAsync(int internalNumber, CancellationToken cancellationToken)
    {
        return await _dbContext.Set<Contact>().Where(c => c.InternalNumber.Equals(internalNumber))
            .FirstOrDefaultAsync(cancellationToken);
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