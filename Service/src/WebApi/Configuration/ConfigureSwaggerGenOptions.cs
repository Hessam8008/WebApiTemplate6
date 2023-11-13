namespace WebApi.Configuration;

using System.Reflection;

using Microsoft.AspNetCore.Mvc.ApiExplorer;
using Microsoft.Extensions.Options;
using Microsoft.OpenApi.Models;

using Swashbuckle.AspNetCore.SwaggerGen;

public class ConfigureSwaggerGenOptions : IConfigureNamedOptions<SwaggerGenOptions>
{
    private readonly IApiVersionDescriptionProvider _provider;

    private readonly SwaggerSettings _settings;

    public ConfigureSwaggerGenOptions(IApiVersionDescriptionProvider provider, IConfiguration configuration)
    {
        this._provider = provider;
        this._settings = SwaggerSettings.GetInstance(configuration);
    }

    /// <summary>
    ///     Configure each API discovered for Swagger Documentation
    /// </summary>
    /// <param name="options"></param>
    public void Configure(SwaggerGenOptions options)
    {
        options.UseDateOnlyTimeOnlyStringConverters();

        this.AddJwtToken(options);

        // Add description and samples for methods, params, etc.
        var presentationXml = GenerateXmlFilePath(Presentation.AssemblyReference.Assembly);
        var applicationXml = GenerateXmlFilePath(Application.AssemblyReference.Assembly);
        options.IncludeXmlComments(presentationXml);
        options.IncludeXmlComments(applicationXml);

        // add swagger document for every API version discovered
        this.GenerateApiDescription(options);
    }

    /// <summary>
    ///     Configure Swagger Options. Inherited from the Interface
    /// </summary>
    /// <param name="name"></param>
    /// <param name="options"></param>
    public void Configure(string name, SwaggerGenOptions options)
    {
        this.Configure(options);
    }

    /// <summary>
    ///     Generates the XML file path.
    /// </summary>
    /// <param name="assembly">The assembly.</param>
    /// <returns>Path of the [AssemblyName.xml] include folder and file name.</returns>
    private static string GenerateXmlFilePath(Assembly assembly)
    {
        var xmlFilename = $"{assembly.GetName().Name}.xml";
        var xmlPath = Path.Combine(AppContext.BaseDirectory, xmlFilename);
        return xmlPath;
    }

    private void AddJwtToken(SwaggerGenOptions options)
    {
        var scopes = this._settings.OAuth2.ScopesArray.ToList();

        options.AddSecurityDefinition(
            "JwtToken",
            new OpenApiSecurityScheme
                {
                    Type = SecuritySchemeType.Http,
                    In = ParameterLocation.Header,
                    Name = "Authorization",
                    Scheme = "Bearer",
                    BearerFormat = "JWT",
                    Description =
                        "JWT Authorization header using the Bearer scheme. \r\n\r\n Enter token in the text input below.",
                });
        options.AddSecurityRequirement(
            new OpenApiSecurityRequirement
                {
                    {
                        new OpenApiSecurityScheme
                            {
                                Reference = new OpenApiReference
                                                {
                                                    Type = ReferenceType.SecurityScheme, Id = "JwtToken"
                                                }
                            },
                        scopes
                    }
                });
    }

    private void AddOAuth2(SwaggerGenOptions options)
    {
        var scopes = this._settings.OAuth2.ScopesArray.ToList();

        options.AddSecurityDefinition(
            "OAuth2",
            new OpenApiSecurityScheme
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
                                                       AuthorizationUrl =
                                                           new Uri(this._settings.OAuth2.AuthorizationUrl),
                                                       TokenUrl = new Uri(this._settings.OAuth2.TokenUrl),
                                                       Scopes = this._settings.OAuth2.Scopes
                                                   }
                                }
                });
        options.AddSecurityRequirement(
            new OpenApiSecurityRequirement
                {
                    {
                        new OpenApiSecurityScheme
                            {
                                Reference = new OpenApiReference { Type = ReferenceType.SecurityScheme, Id = "OAuth2" }
                            },
                        scopes
                    }
                });
    }

    /// <summary>
    ///     Create information about the version of the API
    /// </summary>
    /// <param name="desc"></param>
    /// <returns>Information about the API</returns>
    private OpenApiInfo CreateVersionInfo(ApiVersionDescription desc)
    {
        var info = new OpenApiInfo
                       {
                           Title = this._settings.Doc.Title,
                           Version = desc.ApiVersion.ToString(),
                           Description = this._settings.Doc.Description
                       };

        if (desc.IsDeprecated)
            info.Description += " [Deprecated]";

        return info;
    }

    /// <summary>
    ///     Generates the API description.
    /// </summary>
    /// <param name="options">The swagger options.</param>
    private void GenerateApiDescription(SwaggerGenOptions options)
    {
        foreach (var description in this._provider.ApiVersionDescriptions)
            options.SwaggerDoc(description.GroupName, this.CreateVersionInfo(description));
    }
}