using HealthChecks.UI.Client;
using Microsoft.AspNetCore.Diagnostics.HealthChecks;
using Microsoft.Extensions.Options;

namespace WebApi.Configuration;

internal class ConfigureHealthCheck : IConfigureNamedOptions<HealthCheckOptions>
{
    public void Configure(HealthCheckOptions options)
    {
        options.Predicate = _ => true;
        options.ResponseWriter = UIResponseWriter.WriteHealthCheckUIResponse;
    }

    public void Configure(string name, HealthCheckOptions options)
    {
        Configure(options);
    }
}