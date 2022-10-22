﻿namespace WebApi.Extensions;

#region Configuration objects

internal sealed record SwaggerConfig
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

internal sealed record Document
{
    public string Title { get; set; }

    public string Description { get; set; }

    public string RoutePrefix { get; set; }
}

internal sealed record OAuth2
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