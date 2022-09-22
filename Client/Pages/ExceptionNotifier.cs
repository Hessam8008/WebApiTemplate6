using System.Diagnostics;

namespace Client.Pages;

public class ExceptionNotifier : IExceptionNotifier
{
    public void NotifyException(string message, ExceptionLevel level = ExceptionLevel.Info)
    {
        Debug.WriteLine($"{level}: {message}");
    }
}