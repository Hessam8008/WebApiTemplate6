using System.Text.Json.Serialization;
using Microsoft.AspNetCore.Mvc;

namespace WebApi.Extensions;

public static class JsonOptionsExtension
{
    public static void Configure(this JsonOptions options)
    {
        options.AddJsonSerializerOptions();
        options.AddTypeConvertors();
    }

    private static void AddTypeConvertors(this JsonOptions options)
    {
        options.UseDateOnlyTimeOnlyStringConverters();
    }

    private static void AddJsonSerializerOptions(this JsonOptions options)
    {
        options.JsonSerializerOptions.DefaultIgnoreCondition = JsonIgnoreCondition.WhenWritingNull;
    }
}