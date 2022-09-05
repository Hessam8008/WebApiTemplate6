using System.Diagnostics;
using Microsoft.AspNetCore.Diagnostics;
using Microsoft.AspNetCore.Mvc;
using WebApiTemplate.Domain;

namespace WebApiTemplate.Controllers;

[ApiController]
public class ErrorHandlingController : ControllerBase
{
    [Route("/error")]
    [ApiExplorerSettings(IgnoreApi = true)]
    public IActionResult HandleErrorDevelopment([FromServices] IHostEnvironment hostEnvironment)
    {
        var exceptionHandlerFeature =
            HttpContext.Features.Get<IExceptionHandlerFeature>()!;


        /*---- Business Errors ----*/
        if (exceptionHandlerFeature.Error is DomainException exception)
            return new ObjectResult(new HttpDomainErrorResponse(exception)) {StatusCode = 499};


        /*---- Unhandled Errors ----*/
        var detail = ExtractExceptionDetails(exceptionHandlerFeature, hostEnvironment.IsDevelopment());
        var exResponse = new HttpExceptionResponse(detail);
        return new ObjectResult(exResponse) {StatusCode = StatusCodes.Status500InternalServerError};
    }


    private static List<ExceptionDetails> ExtractExceptionDetails(IExceptionHandlerFeature exFeature,
        bool developmentMode = false)
    {
        var e = exFeature.Error;
        var result = new List<ExceptionDetails>();
        if (!developmentMode)
            result.Add(new ExceptionDetails("An error has occurred. Call the administrator.", null));
        else
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

/* ~~~~~~ Exception ~~~~~~ */
public record HttpExceptionResponse(List<ExceptionDetails> Exceptions);

public record ExceptionDetails(string Message, List<ExceptionFrame>? Frames);

public record ExceptionFrame(string? Method, string? Path, int LineNumber);

/* ~~~~~~~ Domain ~~~~~~~ */
public record HttpDomainErrorResponse
{
    public List<DomainError> Errors { get; } = new();

    public HttpDomainErrorResponse(DomainException exception)
    {
        foreach (var e in exception.Details)
            Errors.Add(new DomainError(e.Message, e.Code));
    }
}

public record DomainError(string Message, int Code = 0);

#endregion
