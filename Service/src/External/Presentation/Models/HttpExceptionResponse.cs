/*
 * Attention: Do NOT remove parameter less constructors.
 *            They are require for serialization.
 */

namespace Presentation.Models;

/// <summary>
///     Root object of the exceptions response.
/// </summary>
/// <param name="Exceptions"></param>
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

/// <summary>
///     Exception message include frames.
/// </summary>
/// <param name="Message"></param>
/// <param name="Frames"></param>
public record ExceptionDetails(string Message, List<ExceptionFrame>? Frames)
{
    public ExceptionDetails() : this(string.Empty, null)
    {
    }
}

/// <summary>
///     Location of the exception occurred.
/// </summary>
/// <param name="Method">Method name</param>
/// <param name="Path">Path of the file.</param>
/// <param name="LineNumber">Line of the error occurred.</param>
public record ExceptionFrame(string? Method, string? Path, int LineNumber)
{
    public ExceptionFrame() : this(null, null, 0)
    {
    }
}