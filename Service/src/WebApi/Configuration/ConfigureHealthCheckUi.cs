using HealthChecks.UI.Configuration;

namespace WebApi.Configuration;

internal class ConfigureHealthCheckUi : Microsoft.Extensions.Options.IConfigureNamedOptions<Options>
{
    public void Configure(Options options)
    {
        options.UIPath = "/hc-ui";
    }

    public void Configure(string name, Options options)
    {
        Configure(options);
    }
}