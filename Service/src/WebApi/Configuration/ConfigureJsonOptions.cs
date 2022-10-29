using System.Text.Json.Serialization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;

namespace WebApi.Configuration;

internal class ConfigureJsonOptions : IConfigureNamedOptions<JsonOptions>
{
    public void Configure(JsonOptions options)
    {
        options.UseDateOnlyTimeOnlyStringConverters();
        options.JsonSerializerOptions.DefaultIgnoreCondition = JsonIgnoreCondition.WhenWritingNull;
    }

    public void Configure(string name, JsonOptions options)
    {
        Configure(options);
    }
}