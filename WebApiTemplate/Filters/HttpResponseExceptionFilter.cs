using System.Diagnostics;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using WebApiTemplate.Domain;

namespace WebApiTemplate.Filters;

public class HttpResponseExceptionFilter : IActionFilter, IOrderedFilter
{
    public int Order => int.MaxValue - 10;

    public void OnActionExecuting(ActionExecutingContext context)
    {
    }

    public void OnActionExecuted(ActionExecutedContext context)
    {
        Debug.WriteLine($"Filter: {context.ActionDescriptor.DisplayName}");

        if (context.Exception is null)
        {
            Debug.WriteLine("Filter: No exception.");
            return;
        }

        if (context.Exception is not DomainException httpResponseException)
            return;

        context.Result = new ObjectResult(httpResponseException.Message)
        {
            StatusCode = 500
        };

        context.ExceptionHandled = true;
    }
}