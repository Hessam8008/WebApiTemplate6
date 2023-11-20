namespace Domain.Abstractions;

public interface ICacheProvider
{
    public Task<T?> GetCache<T>(string key);
    public Task AddCache<T>(string key, T value, int expireInMinutes = 5);
}