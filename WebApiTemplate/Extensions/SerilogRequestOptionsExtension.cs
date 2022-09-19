using System.Security.Claims;
using System.Security.Principal;
using Serilog.AspNetCore;
using Serilog.Events;

namespace WebApiTemplate.Extensions;

public static class SerilogRequestOptionsExtension
{
    public static void Configuration(this RequestLoggingOptions options)
    {
        // Customize the message template
        options.MessageTemplate = "Handled {RequestMethod} {RequestPath} responded {StatusCode} in {Elapsed:0.0000} ms";

        // Emit debug-level events instead of the defaults
        options.GetLevel = (httpContext, elapsed, ex) => LogEventLevel.Information;

        // Attach additional properties to the request completion event
        options.EnrichDiagnosticContext = (diagnosticContext, httpContext) =>
        {
            diagnosticContext.Set("RequestHost", httpContext.Request.Host.Value);
            diagnosticContext.Set("RequestProtocol", httpContext.Request.Protocol);
            diagnosticContext.Set("UserName", GetUserName(httpContext.User));
            diagnosticContext.Set("UserId", GetUserId(httpContext.User));
        };
    }


    private static string? GetUserName(IPrincipal user)
    {
        return user.Identity is {IsAuthenticated: true} ? user.Identity.Name : Environment.UserName;
    }

    private static string? GetUserId(ClaimsPrincipal user)
    {
        return user.Identity is {IsAuthenticated: true} ? user.FindFirstValue("sub") : "Unknown";
    }
}