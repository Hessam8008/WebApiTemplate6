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

    public async Task Insert(Person person)
    {
        await _dbContext.Set<Person>().AddAsync(person);
    }
}