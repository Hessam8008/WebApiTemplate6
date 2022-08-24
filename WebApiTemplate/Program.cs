using System.Net.Mime;
using Microsoft.AspNetCore.Mvc;
using WebApiTemplate.Controllers;

var builder = WebApplication.CreateBuilder(args);

/* Add services to the container */

builder.Services.AddControllers(
        options => { options.Filters.Add<HttpResponseExceptionFilter>(); }
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
    .AddXmlSerializerFormatters();


/* Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle */
builder.Services.AddEndpointsApiExplorer();     //
builder.Services.AddSwaggerGen();               //





/* Build app */
var app = builder.Build();



/* Configure the HTTP request pipeline */

app.UseSwagger();
app.UseSwaggerUI();

if (app.Environment.IsDevelopment())
    app.UseExceptionHandler("/error-development");
else
    app.UseExceptionHandler("/error");

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();
