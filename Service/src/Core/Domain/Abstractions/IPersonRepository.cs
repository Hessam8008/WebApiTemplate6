using Domain.Entities;

namespace Domain.Abstractions;

public interface IPersonRepository
{
    Task InsertAsync(Person person);

    Task<Person?> SelectAsync(Guid id);

    Task UpdateAsync(Person person);

    Task DeleteAsync(Guid id);
}