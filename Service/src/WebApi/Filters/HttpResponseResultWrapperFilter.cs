using System.Diagnostics;
using Domain.Primitives.Result;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;

namespace WebApi.Filters;

public class HttpResponseResultWrapperFilter : IActionFilter, IOrderedFilter
{
    public int Order => int.MaxValue - 10;

    public void OnActionExecuting(ActionExecutingContext context)
    {
    }

    public void OnActionExecuted(ActionExecutedContext context)
    {
        Debug.WriteLine($"Filter: {context.ActionDescriptor.DisplayName}");

        if (context.Exception is not null)
            return;

        if (context.Result is not ObjectResult {Value: Result result} objectResult)
            return;

        if (result.IsFailure)
        {
            context.Result = new ObjectResult(result.Error) {StatusCode = 499};
        }
        else
        {
            object? obj = null;
            var valueType = objectResult.Value.GetType();

            if (valueType.IsGenericType && valueType.GetGenericTypeDefinition() == typeof(Result<>))
                obj = valueType.GetProperty("Value")?.GetValue(objectResult.Value);

            context.Result = new ObjectResult(obj) {StatusCode = objectResult.StatusCode};
        }
    }
}