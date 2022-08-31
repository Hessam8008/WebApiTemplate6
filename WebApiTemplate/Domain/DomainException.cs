namespace WebApiTemplate.Domain;

public class DomainException : Exception
{
    public override string Message { get; }

    public DomainException(string message)
    {
        Message = message;
    }
}