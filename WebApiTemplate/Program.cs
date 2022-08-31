using System.Net.Mime;
using System.Text.Json.Serialization;
using Microsoft.AspNetCore.Mvc;

var builder = WebApplication.CreateBuilder(args);

/* Add services to the container */

builder.Services.AddControllers(
        options =>
        {
            // Add filtering for exceptions
            //options.Filters.Add<HttpResponseExceptionFilter>();
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


if (true || app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseExceptionHandler("/error");

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();
