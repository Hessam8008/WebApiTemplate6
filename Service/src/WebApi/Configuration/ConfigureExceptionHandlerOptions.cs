using System.Diagnostics;
using System.Net.Mime;
using System.Security.Claims;
using Microsoft.AspNetCore.Diagnostics;
using Microsoft.Extensions.Options;
using Newtonsoft.Json;
using Presentation.Models;
using Serilog;

namespace WebApi.Configuration;

internal class ConfigureExceptionHandlerOptions : IConfigureNamedOptions<ExceptionHandlerOptions>
{
    private readonly IHostEnvironment _hostEnvironment;


    public ConfigureExceptionHandlerOptions(IHostEnvironment hostEnvironment)
    {
        _hostEnvironment = hostEnvironment;
    }

    public void Configure(ExceptionHandlerOptions options)
    {
        options.ExceptionHandler += ExceptionHandler;
    }

    private async Task ExceptionHandler(HttpContext context)
    {
        /*───── Unhandled errors handling here ─────*
         *──────────────────────────────────────────*/

        /* ↓ Where exception happened ↓ */
        // var exceptionHandlerPathFeature = context.Features.Get<IExceptionHandlerPathFeature>();

        var exceptionHandlerFeature = context.Features.Get<IExceptionHandlerFeature>()!;
        var user = context.User;

        var log = Log.ForContext<AssemblyReference>()
            .ForContext("UserId", user?.FindFirstValue(ClaimTypes.NameIdentifier))
            .ForContext("UserName", user?.FindFirstValue(CustomClaimTypes.UserName))
            .ForContext("UserTitle", user?.FindFirstValue(ClaimTypes.Name));


        var detail = ExtractExceptionDetails(exceptionHandlerFeature);
        var exResponse = _hostEnvironment.IsDevelopment()
            ? new HttpExceptionResponse(detail)
            : new HttpExceptionResponse(new ExceptionDetails("An error has occurred. Call the administrator.", null));

        log.ForContext("Details", detail, true).Error($"{detail.FirstOrDefault()?.Message ?? "System Error."}");

        var response = JsonConvert.SerializeObject(exResponse);
        context.Response.StatusCode = StatusCodes.Status500InternalServerError;
        context.Response.ContentType = MediaTypeNames.Application.Json;
        await context.Response.WriteAsync(response);
    }

    public void Configure(string name, ExceptionHandlerOptions options)
    {
        Configure(options);
    }


    private static List<ExceptionDetails> ExtractExceptionDetails(IExceptionHandlerFeature exFeature)
    {
        var e = exFeature.Error;
        var result = new List<ExceptionDetails>();


        while (e != null)
        {
            var exDetails = new ExceptionDetails(e.Message, new List<ExceptionFrame>());

            var st = new StackTrace(e, true);
            var frames = st.GetFrames().Where(f => f.GetFileLineNumber() > 0);
            foreach (var frame in frames)
                exDetails.Frames?.Add(
                    new ExceptionFrame(frame.GetMethod()?.Name, frame.GetFileName(), frame.GetFileLineNumber()));

            e = e.InnerException;
            result.Add(exDetails);
        }

        return result;
    }
}