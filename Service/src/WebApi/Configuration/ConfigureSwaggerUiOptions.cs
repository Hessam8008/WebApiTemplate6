using Microsoft.AspNetCore.Mvc.ApiExplorer;
using Microsoft.Extensions.Options;
using Swashbuckle.AspNetCore.SwaggerUI;
using WebApi.Extensions;

namespace WebApi.Configuration;

public class ConfigureSwaggerUiOptions
    : IConfigureNamedOptions<SwaggerUIOptions>
{
    private readonly IApiVersionDescriptionProvider _provider;
    private readonly IConfiguration _configuration;


    public ConfigureSwaggerUiOptions(IApiVersionDescriptionProvider provider, IConfiguration configuration)
    {
        _provider = provider;
        _configuration = configuration;
    }

    public void Configure(SwaggerUIOptions options)
    {
        var config = SwaggerConfig.GetInstance(_configuration);

        options.DefaultModelsExpandDepth(-1);
        if (config.OAuth2.UsePKCE) options.OAuthUsePkce();

        //options.SwaggerEndpoint(config.Doc.JsonEndpoint, config.Doc.Version);
        options.RoutePrefix = config.Doc.RoutePrefix;
        options.OAuthConfigObject = new OAuthConfigObject
        {
            ClientId = config.OAuth2.ClientId,
            Scopes = config.OAuth2.ScopesArray,
            AppName = config.Doc.Title,
            UsePkceWithAuthorizationCodeGrant = config.OAuth2.UsePKCE
        };

        foreach (var description in _provider.ApiVersionDescriptions.Reverse())
            options.SwaggerEndpoint(
                $"/swagger/{description.GroupName}/swagger.json",
                description.GroupName.ToUpperInvariant() + (description.IsDeprecated ? " [Deprecated]" : ""));
    }

    public void Configure(string name, SwaggerUIOptions options)
    {
        Configure(options);
    }
}