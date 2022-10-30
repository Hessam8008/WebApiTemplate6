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

        var options = DatabaseSettings.GetInstance();


        services.AddScoped<IPersonRepository, PeronRepository>();
        services.AddScoped<IUnitOfWork, UnitOfWork>();
        services.AddSingleton<DomainEventsToOutboxInterceptor>();
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
        return services;
    }
}