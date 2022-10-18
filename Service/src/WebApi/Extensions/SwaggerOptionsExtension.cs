using Microsoft.OpenApi.Models;
using Swashbuckle.AspNetCore.SwaggerGen;
using Swashbuckle.AspNetCore.SwaggerUI;

namespace WebApi.Extensions;

internal static class SwaggerOptionsExtension
{
    public static void Configure(this SwaggerGenOptions options, IConfiguration configuration)
    {
        var swaggerConfig = configuration.GetSection(SwaggerConfig.ConfigSection).Get<SwaggerConfig>();

        options.UseDateOnlyTimeOnlyStringConverters();

        options.SwaggerDoc("v1", new OpenApiInfo
        {
            Version = swaggerConfig.Doc.Version,
            Title = "Web API",
            Description = "API services."
        });

        options.AddSecurityDefinition("oauth2", new OpenApiSecurityScheme
        {
            Type = SecuritySchemeType.OAuth2,
            Flows = new OpenApiOAuthFlows
            {
                AuthorizationCode = new OpenApiOAuthFlow
                {
                    AuthorizationUrl = new Uri("https://idp.bxbinv.ir/connect/authorize"),
                    TokenUrl = new Uri("https://idp.bxbinv.ir/connect/token"),
                    Scopes = new Dictionary<string, string>
                    {
                        {"webApi", "Web API"}
                    }
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
                new[] {"webApi"}
            }
        });

        var xmlFilename = $"{Presentation.AssemblyReference.Assembly.GetName().Name}.xml";
        options.IncludeXmlComments(Path.Combine(AppContext.BaseDirectory, xmlFilename));
    }

    public static void Configure(this SwaggerUIOptions options)
    {
        options.DefaultModelsExpandDepth(-1);
        options.OAuthUsePkce();
        options.SwaggerEndpoint("/swagger/v1/swagger.json", "v1");
        options.RoutePrefix = string.Empty;
        options.OAuthConfigObject = new OAuthConfigObject
        {
            ClientId = "trader_swagger",
            Scopes = new[] {"traderApi"},
            AppName = "Swagger for Traders service",
            UsePkceWithAuthorizationCodeGrant = true
        };
    }

    private class SwaggerConfig
    {
        public const string ConfigSection = "Swagger";

        public Doc Doc { get; set; }

        public OAuth2 OAuth2 { get; set; }
    }

    private class Doc
    {
        public string Version { get; set; }

        public string Title { get; set; }

        public string Description { get; set; }
    }

    private class OAuth2
    {
        public string AuthorizationUrl { get; set; }

        public string TokenUrl { get; set; }

        public Dictionary<string, string> Scopes { get; set; }
    }
}