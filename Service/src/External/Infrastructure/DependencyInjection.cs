﻿using Domain.Abstractions;
using Infrastructure.Repositories;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace Infrastructure;

public static class DependencyInjection
{
    public static IServiceCollection AddInfrastructure(this IServiceCollection services,
        ConfigurationManager configuration)
    {
        var options = configuration
            .GetSection(DatabaseOptions.ConfigurationName)
            .Get<DatabaseOptions>();


        services.AddScoped<IPersonRepository, PeronRepository>();
        services.AddScoped<IUnitOfWork, UnitOfWork>();
        services.AddDbContext<ApplicationDbContext>(option =>
        {
            option.UseSqlServer(options.ConnectionString,
                    sqlAction => sqlAction
                        .EnableRetryOnFailure(options.MaxRetryCount)
                        .CommandTimeout(options.CommandTimeOut))
                .EnableDetailedErrors(options.EnableDetailedErrors)
                .EnableSensitiveDataLogging(options.EnableSensitiveDataLogging);
        });
        return services;
    }

    private class DatabaseOptions
    {
        public static string ConfigurationName = nameof(DatabaseOptions);

        public string ConnectionString { get; set; } = string.Empty;
        public int MaxRetryCount { get; set; }
        public int CommandTimeOut { get; set; }
        public bool EnableDetailedErrors { get; set; }
        public bool EnableSensitiveDataLogging { get; set; }
    }
}