using System.Diagnostics;
using System.Security.Claims;
using Microsoft.AspNetCore.Diagnostics;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Hosting;
using Serilog;

namespace Presentation.Controllers;

[ApiController]
public class ErrorHandlingController : ControllerBase
{
    //[ApiVersion("1.0")]
    [Route("error")]
    [ApiExplorerSettings(IgnoreApi = true)]
    public IActionResult HandleErrorDevelopment([FromServices] IHostEnvironment hostEnvironment)
    {
        /*---- Unhandled errors handling here ----*/

        var exceptionHandlerFeature =
            HttpContext.Features.Get<IExceptionHandlerFeature>()!;

        var log = Log.ForContext<ErrorHandlingController>()
            .ForContext("UserId", User.FindFirstValue(ClaimTypes.NameIdentifier))
            .ForContext("UserName", User.FindFirstValue(ClaimTypes.Name));


        var detail = ExtractExceptionDetails(exceptionHandlerFeature);
        var exResponse = hostEnvironment.IsDevelopment()
            ? new HttpExceptionResponse(detail)
            : new HttpExceptionResponse(new ExceptionDetails("An error has occurred. Call the administrator.", null));

        log.ForContext("Details", detail, true).Error($"{detail.FirstOrDefault()?.Message ?? "System Error."}");
        return new ObjectResult(exResponse) {StatusCode = StatusCodes.Status500InternalServerError};
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

#region Exceptions & Errors Responses Records

/*
 * Attention: Do NOT remove parameter less constructors.
 *            They are require for serialization.
 */
public record HttpExceptionResponse(List<ExceptionDetails> Exceptions)
{
    public HttpExceptionResponse() : this(new List<ExceptionDetails>())
    {
    }

    public HttpExceptionResponse(ExceptionDetails exception) : this()
    {
        Exceptions.Add(exception);
    }
}

public record ExceptionDetails(string Message, List<ExceptionFrame>? Frames)
{
    public ExceptionDetails() : this(string.Empty, null)
    {
    }
}

public record ExceptionFrame(string? Method, string? Path, int LineNumber)
{
    public ExceptionFrame() : this(null, null, 0)
    {
    }
}

#endregion
