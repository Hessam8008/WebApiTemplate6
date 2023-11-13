namespace WebApi.Configuration;

using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.Extensions.Options;

internal class ConfigureAuthenticationOptions : IConfigureNamedOptions<AuthenticationOptions>
{
    public void Configure(AuthenticationOptions options)
    {
        this.Configure(options);
    }

    public void Configure(string name, AuthenticationOptions options)
    {
        options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
        options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
    }
}