using Microsoft.AspNetCore.Authorization;

namespace WebApi.Extensions;

internal static class AuthorizationOptionsExtensions
{
    public static void Configure(this AuthorizationOptions options)
    {
        options.AddPolicy("admin", p =>
        {
            p.RequireAuthenticatedUser();
            p.RequireRole("admin");
            p.RequireClaim("role", "admin");
        });
    }
}