using System.Net.Mime;
using System.Text.Json.Serialization;
using Microsoft.AspNetCore.Mvc;
using WebApiTemplate.Controllers;

var builder = WebApplication.CreateBuilder(args);

/* Add services to the container */
builder.Services.AddControllers(
        options =>
        {
            options.Filters.Add(new ProducesResponseTypeAttribute(typeof(void), StatusCodes.Status403Forbidden));
            options.Filters.Add(new ProducesResponseTypeAttribute(typeof(void), StatusCodes.Status401Unauthorized));
            options.Filters.Add(new ProducesResponseTypeAttribute(typeof(void), StatusCodes.Status400BadRequest));
            options.Filters.Add(new ProducesResponseTypeAttribute(typeof(void), StatusCodes.Status404NotFound));
            options.Filters.Add(new ProducesResponseTypeAttribute(typeof(void), StatusCodes.Status410Gone));
            options.Filters.Add(new ProducesResponseTypeAttribute(typeof(HttpDomainErrorResponse),
                ExtraStatusCodes.Status499DomainError));
            options.Filters.Add(new ProducesResponseTypeAttribute(typeof(HttpExceptionResponse),
                StatusCodes.Status500InternalServerError));
        }
    )

    // Validation failure error response 
    .ConfigureApiBehaviorOptions(options =>
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
    })
    .AddJsonOptions(o => o.JsonSerializerOptions.DefaultIgnoreCondition = JsonIgnoreCondition.WhenWritingNull)
    .AddXmlSerializerFormatters();

/* Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle */
builder.Services.AddEndpointsApiExplorer();     //
builder.Services.AddSwaggerGen();               //


/* Build app */
var app = builder.Build();


/* Configure the HTTP request pipeline */
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseExceptionHandler("/error");

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();
