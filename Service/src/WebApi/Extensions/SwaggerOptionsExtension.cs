using Microsoft.OpenApi.Models;
using Swashbuckle.AspNetCore.SwaggerGen;
using Swashbuckle.AspNetCore.SwaggerUI;

namespace WebApi.Extensions;

internal static class SwaggerOptionsExtension
{
    public static void Configure(this SwaggerGenOptions options, IConfiguration configuration)
    {
        var config = SwaggerConfig.GetInstance(configuration);

        options.UseDateOnlyTimeOnlyStringConverters();

        options.SwaggerDoc(config.Doc.Version, new OpenApiInfo
        {
            Version = config.Doc.Version,
            Title = config.Doc.Title,
            Description = config.Doc.Description
        });

        options.AddSecurityDefinition("oauth2", new OpenApiSecurityScheme
        {
            Type = SecuritySchemeType.OAuth2,
            Flows = new OpenApiOAuthFlows
            {
                AuthorizationCode = new OpenApiOAuthFlow
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
        options.IncludeXmlComments(Path.Combine(AppContext.BaseDirectory, xmlFilename));
    }

    public static void Configure(this SwaggerUIOptions options, IConfiguration configuration)
    {
        var config = SwaggerConfig.GetInstance(configuration);

        options.DefaultModelsExpandDepth(-1);
        if (config.OAuth2.UsePKCE) options.OAuthUsePkce();
        options.SwaggerEndpoint(config.Doc.JsonEndpoint, config.Doc.Version);
        options.RoutePrefix = config.Doc.RoutePrefix;
        options.OAuthConfigObject = new OAuthConfigObject
        {
            ClientId = config.OAuth2.ClientId,
            Scopes = config.OAuth2.ScopesArray,
            AppName = config.Doc.Title,
            UsePkceWithAuthorizationCodeGrant = config.OAuth2.UsePKCE
        };
    }

    #region Configuration objects

    private sealed record SwaggerConfig
    {
        private const string ConfigSection = "Swagger";
        private static SwaggerConfig? _singleton;

        public OAuth2 OAuth2 { get; set; }

        public Document Doc { get; set; }

        public static SwaggerConfig GetInstance(IConfiguration configuration)
        {
            return _singleton ??= configuration.GetSection(ConfigSection).Get<SwaggerConfig>();
        }
    }

    private sealed record Document
    {
        public string Version { get; set; }

        public string Title { get; set; }

        public string Description { get; set; }

        public string RoutePrefix { get; set; }

        public string JsonEndpoint { get; set; }
    }

    private sealed record OAuth2
    {
        public string AuthorizationUrl { get; set; }

        public string TokenUrl { get; set; }

        public string ClientId { get; set; }

        public string SecretKey { get; set; }

        public bool UsePKCE { get; set; }

        public Dictionary<string, string> Scopes { get; set; }

        public IEnumerable<string> ScopesArray => Scopes.Keys.ToArray();
    }

    #endregion
}