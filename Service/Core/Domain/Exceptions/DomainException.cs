namespace Domain.Exceptions;

public class DomainException : Exception
{
    public record ExceptionDetails(string Message, int Code = 0);

    public List<ExceptionDetails> Details { get; } = new();

    public override string Message
    {
        get
        {
            var count = Details.Count;
            return count switch
            {
                1 => Details.First().Message,
                > 1 => Details.Select(e => e.Message).Aggregate((a, b) => $"{a}{Environment.NewLine}{b}"),
                _ => "No error found."
            };
        }
    }

    public DomainException()
    {
    }

    public DomainException(string message, int code = 0)
    {
        Details.Add(new ExceptionDetails(message, code));
    }

    public DomainException Add(string message, int code = 0)
    {
        Details.Add(new ExceptionDetails(message, code));
        return this;
    }

    public void ThrowIfNeeded()
    {
        if (Details.Any())
            throw this;
    }
}