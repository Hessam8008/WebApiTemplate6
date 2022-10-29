using Microsoft.AspNetCore.Mvc.ApiExplorer;
using Microsoft.Extensions.Options;

namespace WebApi.Configuration;

public class ConfigureApiExplorerOptions
    : IConfigureNamedOptions<ApiExplorerOptions>
{
    public void Configure(ApiExplorerOptions options)
    {
        options.GroupNameFormat = "'v'VVV";
        options.SubstituteApiVersionInUrl = true;
    }

    public void Configure(string name, ApiExplorerOptions options)
    {
        Configure(options);
    }
}