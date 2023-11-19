using Cache;
using Domain.Abstractions;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Persistence.Interceptors;
using Persistence.Repositories;

namespace Persistence;

public static class DependencyInjection
{
    public static IServiceCollection AddPersistence(this IServiceCollection services,
        ConfigurationManager configuration)
    {
        DatabaseSettings.SetConfiguration(configuration);

        services.AddScoped<IContactRepository, ContactRepository>();
        services.AddScoped<IUnitOfWork, UnitOfWork>();
        services.AddSingleton<DomainEventsToOutboxInterceptor>();
        services.AddSingleton<ICacheProvider, CacheProvider>();

        AddDbContext(services);

        return services;
    }

    private static void AddDbContext(IServiceCollection services)
    {
        var options = DatabaseSettings.GetInstance();

        services.AddDbContext<ApplicationDbContext>((sp, option) =>
        {
            var interceptor = sp.GetService<DomainEventsToOutboxInterceptor>();

            option.UseSqlServer(options.ConnectionString,
                    sqlAction => sqlAction
                        .EnableRetryOnFailure(options.MaxRetryCount)
                        .CommandTimeout(options.CommandTimeOut))
                .EnableDetailedErrors(options.EnableDetailedErrors)
                .EnableSensitiveDataLogging(options.EnableSensitiveDataLogging)
                .AddInterceptors(interceptor);
        });
    }
}