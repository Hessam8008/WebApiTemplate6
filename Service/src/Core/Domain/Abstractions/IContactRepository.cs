using Domain.Entities;

namespace Domain.Abstractions;

public interface IContactRepository
{
    Task InsertAsync(Contact contact, CancellationToken cancellationToken);

    Task<Contact?> SelectAsync(Guid id, CancellationToken cancellationToken);

    Task<List<Contact>> SelectAllAsync(CancellationToken cancellationToken);

    Task<Contact?> SelectByInternalNumberAsync(int internalNumber, CancellationToken cancellationToken);

    void Update(Contact contact);

    void Delete(Contact contact);
}