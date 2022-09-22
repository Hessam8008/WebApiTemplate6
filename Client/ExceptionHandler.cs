using Client.WebApiService;

namespace Client;

public class ExceptionHandler
{
    private readonly IExceptionNotifier? _notifier;

    public ExceptionHandler(IExceptionNotifier? notifier = null)
    {
        _notifier = notifier;
    }

    public async Task ExecuteInTryCatch(
        Func<Task> task,
        Action<Exception>? exceptionRaised = null,
        Action<bool>? waiting = null,
        CancellationTokenSource? cancellationTokenSource = default)
    {
        try
        {
            waiting?.Invoke(true);

            if (cancellationTokenSource?.IsCancellationRequested == true) return;

            if (cancellationTokenSource != null)
                await Task.Run(() => task, cancellationTokenSource.Token).ConfigureAwait(false);
            else
                await Task.Run(task).ConfigureAwait(false);
        }
        catch (Exception exception)
        {
            exceptionRaised?.Invoke(exception);
            HandleException(exception);
        }
        finally
        {
            waiting?.Invoke(false);
        }
    }

    public async Task<T?> ExecuteInTryCatch<T>(
        Func<Task<T>> task,
        Action<Exception>? exceptionRaised = null,
        Action<bool>? waiting = null,
        CancellationTokenSource? cancellationTokenSource = default)
    {
        try
        {
            waiting?.Invoke(true);

            if (cancellationTokenSource?.IsCancellationRequested == true) return default;

            if (cancellationTokenSource != null)
                return await Task.Run(task, cancellationTokenSource.Token).ConfigureAwait(false);
            else
                return await Task.Run(task).ConfigureAwait(false);
        }
        catch (Exception exception)
        {
            exceptionRaised?.Invoke(exception);
            HandleException(exception);
        }
        finally
        {
            waiting?.Invoke(false);
        }

        return default;
    }

    /// <summary>
    ///     Handles the API exception.
    /// </summary>
    /// <param name="exception">The exception.</param>
    private void HandleException(Exception exception)
    {
        switch (exception)
        {
            case ApiException<HttpExceptionResponse> apiException:
                var msg = apiException.Result.Exceptions.Select(e => e.Message)
                    .Aggregate((a, b) => a + Environment.NewLine + b);
                _notifier?.NotifyException(msg, ExceptionLevel.Error);
                break;

            case ApiException<HttpDomainErrorResponse> apiException:
                var msg2 = apiException.Result.Errors.Select(e => e.Message)
                    .Aggregate((a, b) => a + Environment.NewLine + b);
                _notifier?.NotifyException(msg2, ExceptionLevel.Warn);
                break;

            case ApiException apiException:
                var statusCode = apiException.StatusCode;
                var message = statusCode switch
                {
                    400 => "اطلاعات ارسالی صحیح نیست",
                    401 => "دسترسی غیرمجاز است",
                    403 => "دسترسی محدود شده است",
                    404 => "عدم وجود منبع",
                    500 => "خطایی در سمت سرور رخ داده است",
                    _ => $"خطای ناشناخته\n{apiException.Message}"
                };

                _notifier?.NotifyException(message, ExceptionLevel.Error);
                break;

            default:
                _notifier?.NotifyException(exception.Message, ExceptionLevel.Error);
                break;
        }
    }
}