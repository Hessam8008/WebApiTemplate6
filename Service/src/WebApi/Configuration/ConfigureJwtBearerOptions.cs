using System.Text;
using IdentityModel;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;

namespace WebApi.Configuration;

internal class ConfigureJwtBearerOptions : IConfigureNamedOptions<JwtBearerOptions>
{
    private readonly JwtSettings _jwtSettings;

    public ConfigureJwtBearerOptions(IConfiguration configuration)
    {
        _jwtSettings = configuration.GetSection(nameof(JwtSettings)).Get<JwtSettings>() ?? new JwtSettings();
    }

    public void Configure(string name, JwtBearerOptions options)
    {
        options.Authority = _jwtSettings.Authority;
        options.SaveToken = _jwtSettings.SaveToken;
        options.RequireHttpsMetadata = _jwtSettings.RequireHttpsMetadata;
        options.TokenValidationParameters = new TokenValidationParameters
        {
            IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_jwtSettings.IssuerSigningKey)),
            ValidIssuer = _jwtSettings.Issuer,
            ValidateIssuer = _jwtSettings.ValidateIssuer,
            ValidAudience = _jwtSettings.ValidAudience,
            ValidateAudience = _jwtSettings.ValidateAudience,
            NameClaimType = JwtClaimTypes.Name,
            RoleClaimType = JwtClaimTypes.Role
        };
    }

    public void Configure(JwtBearerOptions options)
    {
        Configure(options);
    }


    private class JwtSettings
    {
        public string Authority { get; set; } = "http://10.10.1.103:8300";
        public string? Issuer { get; set; }
        public string IssuerSigningKey { get; set; } = string.Empty;
        public string? ValidAudience { get; set; }
        public bool ValidateIssuer { get; set; }
        public bool SaveToken { get; set; } = true;
        public bool RequireHttpsMetadata { get; set; }
        public bool ValidateAudience { get; set; }
    }
}