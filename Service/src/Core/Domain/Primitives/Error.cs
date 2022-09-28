namespace Domain.Primitives;

public sealed class Error : ValueObject
{
    /// <summary>
    ///     Gets the empty error instance.
    /// </summary>
    public static readonly Error None = new(string.Empty, string.Empty);

    public static readonly Error NullValue = new("Error.NullValue", "The specified result value is null.");


    /// <summary>
    ///     Initializes a new instance of the <see cref="Error" /> class.
    /// </summary>
    /// <param name="code">The error code.</param>
    /// <param name="message">The error message.</param>
    public Error(string code, string message)
    {
        Code = code;
        Message = message;
    }

    /// <summary>
    ///     Gets the error code.
    /// </summary>
    public string Code { get; }

    /// <summary>
    ///     Gets the error message.
    /// </summary>
    public string Message { get; }


    public static implicit operator string(Error error)
    {
        return error?.Code ?? string.Empty;
    }

    /// <inheritdoc />
    protected override IEnumerable<object> GetAtomicValues()
    {
        yield return Code;
        yield return Message;
    }
}