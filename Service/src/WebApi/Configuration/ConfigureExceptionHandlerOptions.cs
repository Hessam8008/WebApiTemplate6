using Microsoft.Extensions.Options;

namespace WebApi.Configuration;

internal class ConfigureExceptionHandlerOptions : IConfigureNamedOptions<ExceptionHandlerOptions>
{
    public void Configure(ExceptionHandlerOptions options)
    {
        options.AllowStatusCode404Response = true;
    }

    public void Configure(string name, ExceptionHandlerOptions options)
    {
        Configure(options);
    }
}