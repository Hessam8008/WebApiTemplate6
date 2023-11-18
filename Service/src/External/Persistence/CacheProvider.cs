using Domain.Abstractions;
using Microsoft.Extensions.Caching.Memory;

namespace Persistence;

internal class CacheProvider : ICacheProvider
{
    private static readonly SemaphoreSlim Semaphore = new(1, 1);
    private readonly IMemoryCache _cache;

    public CacheProvider(IMemoryCache memoryCache) => _cache = memoryCache;

    public async Task<T?> GetCache<T>(string key) => await GetCachedResponse<T>(key);

    public async Task AddCache<T>(string key, T value)
    {
        try
        {
            await Semaphore.WaitAsync();
            var cacheEntryOptions = new MemoryCacheEntryOptions
            {
                AbsoluteExpiration = DateTime.Now.AddMinutes(5),
                SlidingExpiration = TimeSpan.FromMinutes(2),
                Size = 1024
            };
            _cache.Set(key, value, cacheEntryOptions);
        }
        finally
        {
            Semaphore.Release();
        }
    }

    private async Task<T?> GetCachedResponse<T>(string cacheKey)
    {
        var isAvailable = _cache.TryGetValue(cacheKey, out T? result);
        return isAvailable ? result : default;
    }
}