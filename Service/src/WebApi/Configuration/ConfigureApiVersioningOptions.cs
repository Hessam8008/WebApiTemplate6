using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Versioning;
using Microsoft.Extensions.Options;

namespace WebApi.Configuration;

public class ConfigureApiVersioningOptions
    : IConfigureNamedOptions<ApiVersioningOptions>
{
    public void Configure(ApiVersioningOptions options)
    {
        // Add versioning to the APIs 
        options.DefaultApiVersion = new ApiVersion(1, 0);
        options.AssumeDefaultVersionWhenUnspecified = true;
        options.ReportApiVersions = true;
        options.ApiVersionReader =
            ApiVersionReader.Combine(
                new UrlSegmentApiVersionReader(),
                new HeaderApiVersionReader("x-api-version"),
                new MediaTypeApiVersionReader("x-api-version"));
    }

    public void Configure(string name, ApiVersioningOptions options)
    {
        Configure(options);
    }
}