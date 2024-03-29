﻿using Domain.Primitives;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;
using Presentation.Abstractions;
using Presentation.Models;
using WebApi.Filters;

namespace WebApi.Configuration;

public class ConfigureMvcOptions : IConfigureNamedOptions<MvcOptions>
{
    private readonly MvcSettings _mvcSettings;


    public ConfigureMvcOptions(IConfiguration configuration)
    {
        _mvcSettings = configuration.GetSection(nameof(MvcSettings)).Get<MvcSettings>()??new MvcSettings();
    }


    public void Configure(MvcOptions options)
    {
        options.Conventions.Add(new RoutePrefixConvention(_mvcSettings.RouteTemplate));

        // Add filters
        options.Filters.Add<HttpResponseResultWrapperFilter>(); // For Result object

        // Add response types
        options.Filters.Add(new ProducesResponseTypeAttribute(typeof(void),
            StatusCodes.Status403Forbidden));
        options.Filters.Add(new ProducesResponseTypeAttribute(typeof(void),
            StatusCodes.Status401Unauthorized));
        options.Filters.Add(new ProducesResponseTypeAttribute(typeof(Error),
            ExtraStatusCodes.Status499DomainError));
        options.Filters.Add(new ProducesResponseTypeAttribute(typeof(HttpExceptionResponse),
            StatusCodes.Status500InternalServerError));

        //TODO: Remove when work with no problems
        // Add type convertor
        // options.UseDateOnlyTimeOnlyStringConverters();
    }

    public void Configure(string name, MvcOptions options)
    {
        Configure(options);
    }

    private class MvcSettings
    {
        public string RouteTemplate { get; set; } = "api/v{version:apiVersion}/[controller]";
    }
}
