using Domain.Primitives;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;
using Presentation.Controllers;
using WebApi.Filters;

namespace WebApi.Configuration;

public class ConfigureMvcOptions : IConfigureNamedOptions<MvcOptions>
{
    public void Configure(MvcOptions options)
    {
        options.Conventions.Add(
            new RoutePrefixConvention(new RouteAttribute("api/v{version:apiVersion}/[controller]")));

        // Add filters
        options.Filters.Add<HttpResponseResultWrapperFilter>(); // For Result object

        // Add response types
        options.Filters.Add(new ProducesResponseTypeAttribute(typeof(void), StatusCodes.Status403Forbidden));
        options.Filters.Add(new ProducesResponseTypeAttribute(typeof(void), StatusCodes.Status401Unauthorized));
        //options.Filters.Add(new ProducesResponseTypeAttribute(typeof(void), StatusCodes.Status400BadRequest));
        //options.Filters.Add(new ProducesResponseTypeAttribute(typeof(void), StatusCodes.Status404NotFound));
        options.Filters.Add(new ProducesResponseTypeAttribute(typeof(Error),
            ExtraStatusCodes.Status499DomainError));
        options.Filters.Add(new ProducesResponseTypeAttribute(typeof(HttpExceptionResponse),
            StatusCodes.Status500InternalServerError));

        // Add type convertor
        options.UseDateOnlyTimeOnlyStringConverters();
    }

    public void Configure(string name, MvcOptions options)
    {
        Configure(options);
    }
}