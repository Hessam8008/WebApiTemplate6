using Domain.Primitives;
using Microsoft.AspNetCore.Mvc;
using Presentation.Controllers;
using WebApi.Filters;

namespace WebApi.Extensions;

public static class MvcOptionsExtension
{
    public static void Configure(this MvcOptions options)
    {
        options.AddStatusCodesFilters();
        options.AddTypeConvertors();
    }

    private static void AddStatusCodesFilters(this MvcOptions options)
    {
        // Add filters

        options.Filters.Add<HttpResponseResultWrapperFilter>(); // For Result object

        // Add response types
        //options.Filters.Add(new ProducesResponseTypeAttribute(typeof(void), StatusCodes.Status403Forbidden));
        //options.Filters.Add(new ProducesResponseTypeAttribute(typeof(void), StatusCodes.Status401Unauthorized));
        //options.Filters.Add(new ProducesResponseTypeAttribute(typeof(void), StatusCodes.Status400BadRequest));
        //options.Filters.Add(new ProducesResponseTypeAttribute(typeof(void), StatusCodes.Status404NotFound));
        options.Filters.Add(new ProducesResponseTypeAttribute(typeof(Error),
            ExtraStatusCodes.Status499DomainError));
        options.Filters.Add(new ProducesResponseTypeAttribute(typeof(HttpExceptionResponse),
            StatusCodes.Status500InternalServerError));
    }

    private static void AddTypeConvertors(this MvcOptions options)
    {
        options.UseDateOnlyTimeOnlyStringConverters();
    }
}