namespace WebApi.Configuration;

using Microsoft.AspNetCore.Mvc.ApiExplorer;
using Microsoft.Extensions.Options;

public class ConfigureApiExplorerOptions : IConfigureNamedOptions<ApiExplorerOptions>
{
    public void Configure(ApiExplorerOptions options)
    {
        options.GroupNameFormat = "'v'VVV";
        options.SubstituteApiVersionInUrl = true;
    }

    public void Configure(string name, ApiExplorerOptions options)
    {
        this.Configure(options);
    }
}