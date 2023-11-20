using Domain.Abstractions;
using Microsoft.Extensions.DependencyInjection;

namespace Cache;

public static class DependencyInjection
{
    public static IServiceCollection AddCache(this IServiceCollection services)
    {
        /* Add in-memory cache */
        services.AddMemoryCache();
        services.AddSingleton<ICacheProvider, CacheProvider>();

        return services;
    }
}