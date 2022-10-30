using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Diagnostics.HealthChecks;

namespace Persistence;

public static class SqlConnectionHealthCheckExtension
{
    public static IHealthChecksBuilder AddSqlConnectionHealthCheck(this IHealthChecksBuilder hc)
    {
        var options = DatabaseSettings.GetInstance();

        hc.AddCheck(
            "sqlDatabase",
            new SqlConnectionHealthCheck(options.ConnectionString),
            HealthStatus.Unhealthy,
            new[] {"SQL", "DB"});
        return hc;
    }
}