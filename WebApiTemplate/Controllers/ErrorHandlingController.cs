using System.Diagnostics;
using Microsoft.AspNetCore.Diagnostics;
using Microsoft.AspNetCore.Mvc;
using WebApiTemplate.Domain;

namespace WebApiTemplate.Controllers;

[ApiController]
public class ErrorHandlingController : ControllerBase
{
    private readonly ILogger<ErrorHandlingController> _logger;


    public ErrorHandlingController(ILogger<ErrorHandlingController> logger)
    {
        _logger = logger;
    }

    [Route("/error")]
    [ApiExplorerSettings(IgnoreApi = true)]
    public IActionResult HandleErrorDevelopment([FromServices] IHostEnvironment hostEnvironment)
    {
        var exceptionHandlerFeature =
            HttpContext.Features.Get<IExceptionHandlerFeature>()!;

        /*---- Business Errors ----*/
        if (exceptionHandlerFeature.Error is DomainException exception)
        {
            _logger.LogError($"{exception.Message}. Details: {{@Details}}", exception.Details);
            return new ObjectResult(new HttpDomainErrorResponse(exception)) {StatusCode = 499};
        }


        /*---- Unhandled Errors ----*/
        var detail = ExtractExceptionDetails(exceptionHandlerFeature);
        var exResponse = hostEnvironment.IsDevelopment()
            ? new HttpExceptionResponse(detail)
            : new HttpExceptionResponse(new ExceptionDetails("An error has occurred. Call the administrator.", null));

        _logger.LogError($"{detail.FirstOrDefault()?.Message ?? "System Error."} Details: {{@Details}}", detail);

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
