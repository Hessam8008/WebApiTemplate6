using Domain.Abstractions;
using Domain.Entities;

namespace Infrastructure.Repositories;

public class PeronRepository : IPersonRepository
{
    private readonly ApplicationDbContext _dbContext;


    public PeronRepository(ApplicationDbContext dbContext)
    {
        _dbContext = dbContext;
    }

    public async Task InsertAsync(Person person)
    {
        await _dbContext.Set<Person>().AddAsync(person);
    }

    public Task<Person?> SelectAsync(Guid id)
    {
        throw new NotImplementedException();
    }

    public Task UpdateAsync(Person person)
    {
        throw new NotImplementedException();
    }

    public Task DeleteAsync(Guid id)
    {
        throw new NotImplementedException();
    }
}