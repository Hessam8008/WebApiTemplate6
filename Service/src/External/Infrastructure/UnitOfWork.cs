using Domain.Abstractions;

namespace Infrastructure
{
    public sealed class UnitOfWork : IUnitOfWork
    {
        public Task<int> SaveChangesAsync(CancellationToken cancellationToken = default)
        {
            throw new NotImplementedException();
        }
    }
}
