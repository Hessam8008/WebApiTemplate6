using Microsoft.AspNetCore.Mvc.ApiExplorer;
using Microsoft.Extensions.Options;
using Microsoft.OpenApi.Models;
using Swashbuckle.AspNetCore.SwaggerGen;
using WebApi.Extensions;

namespace WebApi.Configuration;

public class ConfigureSwaggerGenOptions
    : IConfigureNamedOptions<SwaggerGenOptions>
{
    private readonly IApiVersionDescriptionProvider _provider;
    private readonly IConfiguration _configuration;

    public ConfigureSwaggerGenOptions(
        IApiVersionDescriptionProvider provider, IConfiguration configuration)
    {
        _provider = provider;
        _configuration = configuration;
    }

    /// <summary>
    ///     Configure each API discovered for Swagger Documentation
    /// </summary>
    /// <param name="options"></param>
    public void Configure(SwaggerGenOptions options)
    {
        var config = SwaggerSettings.GetInstance(_configuration);

        options.UseDateOnlyTimeOnlyStringConverters();
        options.AddSecurityDefinition("oauth2", new OpenApiSecurityScheme
        {
            Type = SecuritySchemeType.OAuth2,
            Flows = new OpenApiOAuthFlows
            {
                /* ► Note:  Grant types list cannot contain both
                 *          implicit and authorization_code
                 -------------------------------------------------*/
                /* For 'authorization_code' uncomment below lines */
                /*AuthorizationCode = new OpenApiOAuthFlow
                {
                    AuthorizationUrl = new Uri(config.OAuth2.AuthorizationUrl),
                    TokenUrl = new Uri(config.OAuth2.TokenUrl),
                    Scopes = config.OAuth2.Scopes
                }
                */
                /* For 'implicit' use the below code */
                Implicit = new OpenApiOAuthFlow
                {
                    AuthorizationUrl = new Uri(config.OAuth2.AuthorizationUrl),
                    TokenUrl = new Uri(config.OAuth2.TokenUrl),
                    Scopes = config.OAuth2.Scopes
                }
            }
        });

        options.AddSecurityRequirement(new OpenApiSecurityRequirement
        {
            {
                new OpenApiSecurityScheme
                {
                    Reference = new OpenApiReference {Type = ReferenceType.SecurityScheme, Id = "oauth2"}
                },
                config.OAuth2.ScopesArray.ToList()
            }
        });

        var xmlFilename = $"{Presentation.AssemblyReference.Assembly.GetName().Name}.xml";
        var xmlPath = Path.Combine(AppContext.BaseDirectory, xmlFilename);
        options.IncludeXmlComments(xmlPath);

        // add swagger document for every API version discovered
        foreach (var description in _provider.ApiVersionDescriptions)
            options.SwaggerDoc(
                description.GroupName,
                CreateVersionInfo(description));
    }

    /// <summary>
    ///     Configure Swagger Options. Inherited from the Interface
    /// </summary>
    /// <param name="name"></param>
    /// <param name="options"></param>
    public void Configure(string name, SwaggerGenOptions options)
    {
        Configure(options);
    }

    /// <summary>
    ///     Create information about the version of the API
    /// </summary>
    /// <param name="description"></param>
    /// <returns>Information about the API</returns>
    private OpenApiInfo CreateVersionInfo(
        ApiVersionDescription desc)
    {
        var config = SwaggerSettings.GetInstance(_configuration);

        var info = new OpenApiInfo
        {
            Title = config.Doc.Title,
            Version = desc.ApiVersion.ToString(),
            Description = config.Doc.Description
        };

        if (desc.IsDeprecated)
            info.Description += " [Deprecated]";

        return info;
    }
}