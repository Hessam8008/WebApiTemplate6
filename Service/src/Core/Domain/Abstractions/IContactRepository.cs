using Domain.Entities;

namespace Domain.Abstractions;

public interface IContactRepository
{
    Task InsertAsync(Contact contact, CancellationToken cancellationToken = default);

    Task<Contact?> SelectAsync(Guid id, CancellationToken cancellationToken = default);

    Task<List<Contact>> SelectAllAsync(CancellationToken cancellationToken = default);

    Task<Contact?> SelectByInternalNumberAsync(short internalNumber, CancellationToken cancellationToken = default);

    Task<bool> ExistsInternalNumber(short internalNumber, CancellationToken cancellationToken = default);

    void Update(Contact contact);

    void Delete(Contact contact);
}