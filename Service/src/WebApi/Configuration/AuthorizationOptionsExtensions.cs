using Microsoft.AspNetCore.Authorization;
using Microsoft.Extensions.Options;

namespace WebApi.Configuration;

internal class ConfigureAuthorizationOptions : IConfigureNamedOptions<AuthorizationOptions>
{
    public void Configure(string name, AuthorizationOptions options)
    {
        options.AddPolicy("admin", p =>
        {
            p.RequireAuthenticatedUser();
            p.RequireRole("SuperAdmin");
            p.RequireClaim("role", "SuperAdmin");
        });

        options.AddPolicy("user", p =>
        {
            p.RequireAuthenticatedUser();
            p.RequireRole("SuperAdmin");
        });
    }

    public void Configure(AuthorizationOptions options)
    {
        Configure(options);
    }
}