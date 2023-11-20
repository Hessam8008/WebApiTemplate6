using Microsoft.AspNetCore.OutputCaching;
using Microsoft.Extensions.Options;

namespace WebApi.Configuration;

internal class ConfigureOutputCacheOptions : IConfigureNamedOptions<OutputCacheOptions>
{
    public void Configure(OutputCacheOptions options)
    {
        options.AddBasePolicy(p => p.Expire(TimeSpan.FromMinutes(20)));
        options.AddPolicy("1Day", p =>
        {
            p.Expire(TimeSpan.FromDays(1));
            p.Tag("1Day_tag");
        });
    }

    public void Configure(string? name, OutputCacheOptions options) => Configure(options);
}