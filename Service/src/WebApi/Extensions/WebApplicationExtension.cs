using Application;
using Persistence;
using Serilog;
using WebApi.Configuration;

namespace WebApi.Extensions;

public static class WebApplicationExtension
{
    /// <summary>
    ///     Configure services. At the end of the method, builder.Build() calls.
    /// </summary>
    /// <param name="builder">Web application builder</param>
    public static void ConfigureServices(this WebApplicationBuilder builder)
    {
        /* Add services to the container */
        builder.Services
            .AddControllers()
            .AddApplicationPart(Presentation.AssemblyReference.Assembly)
            .AddJsonOptions(options => options.Configure());


        builder.Services.AddHttpContextAccessor();

        /* Add version to the API */
        builder.Services.AddApiVersioning();
        builder.Services.AddVersionedApiExplorer();
        builder.Services.ConfigureOptions<ConfigureApiVersioningOptions>();
        builder.Services.ConfigureOptions<ConfigureApiExplorerOptions>();
        builder.Services.ConfigureOptions<ConfigureApiBehaviorOptions>();
        builder.Services.ConfigureOptions<ConfigureMvcOptions>();
        builder.Services.ConfigureOptions<ConfigureAuthorizationOptions>();
        builder.Services.ConfigureOptions<ConfigureJwtBearerOptions>();


        builder.Services.AddEndpointsApiExplorer();
        builder.Services.AddSwaggerGen();
        builder.Services.ConfigureOptions<ConfigureSwaggerGenOptions>();
        builder.Services.ConfigureOptions<ConfigureSwaggerUiOptions>();


        builder.Services.AddApplication();
        builder.Services.AddPersistence(builder.Configuration);

        builder.Services.AddOutboxMessagePublisher();

        builder.Services.AddHealthChecks()
            .AddSqlConnectionHealthCheck();

        builder.Services.AddHealthChecksUI()
            .AddInMemoryStorage();


        builder.Services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
            .AddJwtBearer(JwtBearerDefaults.AuthenticationScheme,
                options => options.Configure());

        builder.Services.AddAuthorization(options => options.Configure());

        /* Log configuration */
        builder.Host.UseSerilog((ctx, lc) =>
            lc.ReadFrom.Configuration(ctx.Configuration));
    }


    public static void ConfigurePipeline(this WebApplication app)
    {
        /* Configure the HTTP request pipeline */
        app.UseSerilogRequestLogging(options => options.Configuration());

        if (app.Environment.IsDevelopment())
        {
            app.UseSwagger();
            app.UseSwaggerUI();
        }

        app.UseExceptionHandler("/error");

        app.UseHsts();

        //app.UseHttpsRedirection();

        //app.UseAuthentication();

        //app.UseAuthorization();

        //app.MapHealthChecks("/hc", new HealthCheckOptions
        //{
        //    Predicate = _ => true,
        //    ResponseWriter = UIResponseWriter.WriteHealthCheckUIResponse
        //});

        //app.MapHealthChecksUI(c => c.UIPath = "/hc-ui");


        app.MapControllers()
            .RequireAuthorization();
    }
}