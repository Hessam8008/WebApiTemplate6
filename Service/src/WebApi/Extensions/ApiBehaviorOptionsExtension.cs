using System.Net.Mime;
using Microsoft.AspNetCore.Mvc;

namespace WebApi.Extensions;

public static class ApiBehaviorOptionsExtension
{
    public static void Configure(this ApiBehaviorOptions options)
    {
        options.InvalidModelStateResponseFactory = context =>
            new BadRequestObjectResult(context.ModelState)
            {
                ContentTypes =
                {
                    // using static System.Net.Mime.MediaTypeNames;
                    MediaTypeNames.Application.Json,
                    MediaTypeNames.Application.Xml
                }
            };
    }
}