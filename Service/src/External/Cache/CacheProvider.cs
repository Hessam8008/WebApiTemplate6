using Domain.Abstractions;
using Microsoft.Extensions.Caching.Memory;

namespace Cache;

internal class CacheProvider : ICacheProvider
{
    private static readonly SemaphoreSlim Semaphore = new(1, 1);
    private readonly IMemoryCache _cache;

    public CacheProvider(IMemoryCache memoryCache) => _cache = memoryCache;

    public async Task<T?> GetCache<T>(string key) => await GetCachedResponse<T>(key);

    public async Task AddCache<T>(string key, T value, int expireInMinutes = 5)
    {
        try
        {
            await Semaphore.WaitAsync();

            var cacheEntryOptions = new MemoryCacheEntryOptions
            {
                AbsoluteExpiration = DateTime.Now.AddMinutes(expireInMinutes),
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

    private Task<T?> GetCachedResponse<T>(string cacheKey)
    {
        var isAvailable = _cache.TryGetValue(cacheKey, out T? result);
        return Task.FromResult(isAvailable ? result : default);
    }
}