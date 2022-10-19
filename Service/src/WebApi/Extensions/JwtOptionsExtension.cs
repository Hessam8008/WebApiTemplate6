using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;

namespace WebApi.Extensions;

internal static class JwtOptionsExtension
{
    public static void Configure(this JwtBearerOptions options)
    {
        options.Authority = "http://10.10.1.103:8300";
        options.SaveToken = true;
        options.RequireHttpsMetadata = false;
        options.TokenValidationParameters = new TokenValidationParameters
        {
            ValidateIssuer = false,
            ValidateAudience = false
        };
    }
}