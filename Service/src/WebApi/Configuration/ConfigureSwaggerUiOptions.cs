namespace WebApi.Configuration;

using Microsoft.AspNetCore.Mvc.ApiExplorer;
using Microsoft.Extensions.Options;

using Swashbuckle.AspNetCore.SwaggerUI;

public class ConfigureSwaggerUiOptions : IConfigureNamedOptions<SwaggerUIOptions>
{
    private readonly IConfiguration _configuration;

    private readonly IApiVersionDescriptionProvider _provider;

    public ConfigureSwaggerUiOptions(IApiVersionDescriptionProvider provider, IConfiguration configuration)
    {
        this._provider = provider;
        this._configuration = configuration;
    }

    public void Configure(SwaggerUIOptions options)
    {
        var config = SwaggerSettings.GetInstance(this._configuration);

        options.DefaultModelsExpandDepth(-1);
        if (config.OAuth2.UsePKCE) options.OAuthUsePkce();

        // options.SwaggerEndpoint(config.Doc.JsonEndpoint, config.Doc.Version);
        options.RoutePrefix = config.Doc.RoutePrefix;
        options.OAuthConfigObject = new OAuthConfigObject
                                        {
                                            ClientId = config.OAuth2.ClientId,
                                            Scopes = config.OAuth2.ScopesArray,
                                            AppName = config.Doc.Title,
                                            UsePkceWithAuthorizationCodeGrant = config.OAuth2.UsePKCE
                                        };

        foreach (var description in this._provider.ApiVersionDescriptions.Reverse())
            options.SwaggerEndpoint(
                $"/swagger/{description.GroupName}/swagger.json",
                description.GroupName.ToUpperInvariant() + (description.IsDeprecated ? " [Deprecated]" : string.Empty));
    }

    public void Configure(string name, SwaggerUIOptions options)
    {
        this.Configure(options);
    }
}