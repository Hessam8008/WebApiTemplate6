namespace WebApiTemplate.Extensions;

public static class WebApplicationExtension
{
    public static void ConfigureServices(this WebApplicationBuilder builder)
    {
        /* Add services to the container */
        builder.Services
            .AddControllers(options => options.Configure())
            .ConfigureApiBehaviorOptions(options => options.Configure())
            .AddJsonOptions(options => options.Configure())
            .AddXmlSerializerFormatters();

        /* Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle */
        builder.Services.AddEndpointsApiExplorer(); //
        builder.Services.AddSwaggerGen(options => options.Configure());
    }


    public static void ConfigurePipeline(this WebApplication app)
    {
        /* Configure the HTTP request pipeline */
        if (app.Environment.IsDevelopment())
        {
            app.UseSwagger();
            app.UseSwaggerUI(options => options.Configure());
        }

        app.UseExceptionHandler("/error");

        app.UseHttpsRedirection();

        app.UseAuthorization();

        app.MapControllers();
    }
}