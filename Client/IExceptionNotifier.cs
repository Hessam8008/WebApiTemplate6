namespace Client;

public interface IExceptionNotifier
{
    void NotifyException(string message, ExceptionLevel level = ExceptionLevel.Info);
}