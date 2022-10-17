using System.Security.Claims;
using Serilog.AspNetCore;
using Serilog.Events;

namespace WebApi.Extensions;

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
            if (httpContext.Request.QueryString.HasValue)
                diagnosticContext.Set("QueryString", httpContext.Request.QueryString.Value);

            diagnosticContext.Set("RequestHost", httpContext.Request.Host.Value);
            diagnosticContext.Set("RequestProtocol", httpContext.Request.Protocol);
            diagnosticContext.Set("UserName", httpContext.User.FindFirstValue(ClaimTypes.NameIdentifier));
            diagnosticContext.Set("UserId", httpContext.User.FindFirstValue(ClaimTypes.Name));
        };
    }
}