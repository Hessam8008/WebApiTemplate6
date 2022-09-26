using Domain.Entities;

namespace Domain.Abstractions;

public interface IPersonRepository
{
    Task Insert(Person person);
}