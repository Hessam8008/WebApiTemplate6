using Application;
using HealthChecks.UI.Client;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Diagnostics.HealthChecks;
using Persistence;
using Publisher;
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
        /* Add configuration */
        builder.Services.ConfigureOptions<ConfigureApiVersioningOptions>();
        builder.Services.ConfigureOptions<ConfigureApiExplorerOptions>();
        builder.Services.ConfigureOptions<ConfigureApiBehaviorOptions>();
        builder.Services.ConfigureOptions<ConfigureMvcOptions>();
        builder.Services.ConfigureOptions<ConfigureAuthorizationOptions>();
        builder.Services.ConfigureOptions<ConfigureJwtBearerOptions>();
        builder.Services.ConfigureOptions<ConfigureSerilogOptions>();
        builder.Services.ConfigureOptions<ConfigureJsonOptions>();
        builder.Services.ConfigureOptions<ConfigureSwaggerGenOptions>();
        builder.Services.ConfigureOptions<ConfigureSwaggerUiOptions>();


        /* Add controllers */
        builder.Services
            .AddControllers()
            .AddApplicationPart(Presentation.AssemblyReference.Assembly);

        builder.Services.AddHttpContextAccessor();

        /* Add versioning */
        builder.Services.AddApiVersioning();
        builder.Services.AddVersionedApiExplorer();

        /* Add Swagger */
        builder.Services.AddEndpointsApiExplorer();
        builder.Services.AddSwaggerGen();

        /* Add application layer */
        builder.Services.AddApplication();

        /* Add persistence layer */
        builder.Services.AddPersistence(builder.Configuration);

        /* Add event publisher (Outbox pattern) */
        builder.Services.AddOutboxMessagePublisher();

        /* Add health check */
        builder.Services.AddHealthChecks()
            .AddSqlConnectionHealthCheck();

        builder.Services.AddHealthChecksUI()
            .AddInMemoryStorage();


        /* Add authentication and authorization */
        builder.Services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
            .AddJwtBearer();

        builder.Services.AddAuthorization();

        /* Add log configuration */
        builder.Host.UseSerilog((ctx, lc) =>
            lc.ReadFrom.Configuration(ctx.Configuration));
    }


    public static void ConfigurePipeline(this WebApplication app)
    {
        /* Configure the HTTP request pipeline */
        app.UseSerilogRequestLogging();

        if (app.Environment.IsDevelopment())
        {
            app.UseSwagger();
            app.UseSwaggerUI();
        }

        app.UseExceptionHandler("/error");

        app.UseHsts();

        app.UseHttpsRedirection();

        app.UseAuthentication();

        app.UseAuthorization();

        app.MapHealthChecks("/hc", new HealthCheckOptions
        {
            Predicate = _ => true,
            ResponseWriter = UIResponseWriter.WriteHealthCheckUIResponse
        });

        app.MapHealthChecksUI(c => c.UIPath = "/hc-ui");


        app.MapControllers()
            .RequireAuthorization();
    }
}