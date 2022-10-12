using Domain.Abstractions;
using Domain.Entities;
using Microsoft.EntityFrameworkCore;

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

    public async Task<Person?> SelectAsync(Guid id)
    {
        return await _dbContext.Set<Person>().FirstOrDefaultAsync();
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