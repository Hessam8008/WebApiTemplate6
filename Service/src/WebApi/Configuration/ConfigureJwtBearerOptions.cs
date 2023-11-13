﻿namespace WebApi.Configuration;

using System.Text;

using IdentityModel;

using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;

using Presentation.Models;

internal class ConfigureJwtBearerOptions : IConfigureNamedOptions<JwtBearerOptions>
{
    private readonly JwtSettings _jwtSettings;

    public ConfigureJwtBearerOptions(IOptions<JwtSettings> jwtSettingOptions)
    {
        _jwtSettings = jwtSettingOptions.Value;
    }

    public void Configure(string name, JwtBearerOptions options)
    {
        var symmetricSecurityKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_jwtSettings.IssuerSigningKey));

        /* Authority url for authorization token */
        // options.Authority = _jwtSettings.Authority;
        options.SaveToken = _jwtSettings.SaveToken;
        options.RequireHttpsMetadata = _jwtSettings.RequireHttpsMetadata;
        options.TokenValidationParameters = new TokenValidationParameters
                                                {
                                                    IssuerSigningKey = symmetricSecurityKey,
                                                    ValidIssuer = _jwtSettings.Issuer,
                                                    ValidateIssuer = _jwtSettings.ValidateIssuer,
                                                    ValidAudience = _jwtSettings.ValidAudience,
                                                    ValidateAudience = _jwtSettings.ValidateAudience,
                                                    ValidateLifetime = _jwtSettings.ValidateLifetime,
                                                    ValidateIssuerSigningKey = _jwtSettings.ValidateIssuerSigningKey,
                                                    NameClaimType = JwtClaimTypes.Name,
                                                    RoleClaimType = JwtClaimTypes.Role
                                                };
    }

    public void Configure(JwtBearerOptions options)
    {
        Configure(options);
    }
}