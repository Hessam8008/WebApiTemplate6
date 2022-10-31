using Microsoft.Extensions.Options;
using WebApi.Configuration;

namespace WebApi.Extensions;

public static class HealthCheckExtension
{
    public static IEndpointConventionBuilder UseHealthCheck(this IEndpointRouteBuilder app)
    {
        var uiOptions = app.ServiceProvider.GetRequiredService<IOptions<ConfigureHealthCheckUi>>();
        app.MapHealthChecks("/hc");
        return app.MapHealthChecksUI(o => { uiOptions.Value.Configure(o); });
    }
}