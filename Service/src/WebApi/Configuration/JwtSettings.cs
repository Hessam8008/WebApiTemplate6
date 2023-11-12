namespace WebApi.Configuration;


public class JwtSettings
{
    public string? IssuerSigningKey { get; set; }
    public string? Authority { get; set; }
    public string? Issuer { get; set; }
    public string? ValidAudience { get; set; }
    public bool ValidateAudience { get; set; }
    public bool ValidateIssuer { get; set; }
    public bool ValidateLifetime { get; set; }
    public bool ValidateIssuerSigningKey { get; set; }
    public bool SaveToken { get; set; }
    public bool RequireHttpsMetadata { get; set; }
}
