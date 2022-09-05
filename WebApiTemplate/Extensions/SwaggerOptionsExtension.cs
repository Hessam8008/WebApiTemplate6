using Swashbuckle.AspNetCore.SwaggerGen;
using Swashbuckle.AspNetCore.SwaggerUI;

namespace WebApiTemplate.Extensions;

public static class SwaggerOptionsExtension
{
    public static void Configure(this SwaggerGenOptions options)
    {
        options.AddTypeConvertors();
    }

    private static void AddTypeConvertors(this SwaggerGenOptions options)
    {
        options.UseDateOnlyTimeOnlyStringConverters();
    }

    public static void Configure(this SwaggerUIOptions options)
    {
        options.DefaultModelsExpandDepth(-1);
    }
}