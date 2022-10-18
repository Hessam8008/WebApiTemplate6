using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;

namespace WebApi.Extensions;

internal static class JwtOptionsExtension
{
    public static void Configure(this JwtBearerOptions options)
    {
        options.Authority = "https://idp.golrizpaper.com";
        options.SaveToken = true;
        options.TokenValidationParameters = new TokenValidationParameters
        {
            ValidAudience = "",
            ValidateIssuer = false,
            ValidateAudience = false
        };
    }
}