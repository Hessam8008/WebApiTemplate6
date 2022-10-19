using Microsoft.OpenApi.Models;
using Swashbuckle.AspNetCore.SwaggerGen;
using Swashbuckle.AspNetCore.SwaggerUI;

namespace WebApi.Extensions;

internal static class SwaggerOptionsExtension
{
    public static void Configure(this SwaggerGenOptions options)
    {
        options.UseDateOnlyTimeOnlyStringConverters();

        options.SwaggerDoc("v1", new OpenApiInfo
        {
            Version = "v1",
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
        options.RoutePrefix = "";
        options.OAuthConfigObject = new OAuthConfigObject
        {
            ClientId = "trader_swagger",
            Scopes = new[] {"traderApi"},
            AppName = "Swagger for Traders service",
            UsePkceWithAuthorizationCodeGrant = true
        };
    }
}