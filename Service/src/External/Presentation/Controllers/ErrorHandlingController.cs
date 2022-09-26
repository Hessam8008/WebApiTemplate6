using System.Diagnostics;
using System.Security.Claims;
using Domain.Exceptions;
using Microsoft.AspNetCore.Diagnostics;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Hosting;
using Serilog;

namespace Presentation.Controllers;

[ApiController]
public class ErrorHandlingController : ControllerBase
{
    [Route("/error")]
    [ApiExplorerSettings(IgnoreApi = true)]
    public IActionResult HandleErrorDevelopment([FromServices] IHostEnvironment hostEnvironment)
    {
        var exceptionHandlerFeature =
            HttpContext.Features.Get<IExceptionHandlerFeature>()!;

        var log = Log.ForContext<ErrorHandlingController>()
            .ForContext("UserId", User.FindFirstValue(ClaimTypes.NameIdentifier))
            .ForContext("UserName", User.FindFirstValue(ClaimTypes.Name));

        /*---- Business Errors ----*/
        if (exceptionHandlerFeature.Error is DomainException exception)
        {
            log.ForContext("Details", exception.Details, true).Warning(exception.Message);
            return new ObjectResult(new HttpDomainErrorResponse(exception)) {StatusCode = 499};
        }


        /*---- Unhandled Errors ----*/
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
            var error = new ExceptionDetails(e.Message, new List<ExceptionFrame>());

            var st = new StackTrace(e, true);
            var frames = st.GetFrames().Where(f => f.GetFileLineNumber() > 0);
            foreach (var frame in frames)
                error.Frames?.Add(
                    new ExceptionFrame(frame.GetMethod()?.Name, frame.GetFileName(), frame.GetFileLineNumber()));

            e = e.InnerException;
            result.Add(error);
        }

        return result;
    }
}

#region Exceptions & Errors Responses Records

/*
 * Attention: Do NOT remove parameter less constructors.
 *            They are require for serialization.
 */

/* ~~~~~~ Exception ~~~~~~ */
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

/* ~~~~~~~ Domain ~~~~~~~ */
public record HttpDomainErrorResponse
{
    public List<DomainError> Errors { get; } = new();

    public HttpDomainErrorResponse()
    {
    }

    public HttpDomainErrorResponse(DomainException exception)
    {
        foreach (var e in exception.Details)
            Errors.Add(new DomainError(e.Message, e.Code));
    }
}

public record DomainError(string Message, int Code = 0)
{
    public DomainError() : this(string.Empty)
    {
    }
}

#endregion
