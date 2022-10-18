﻿using Application;
using HealthChecks.UI.Client;
using Microsoft.AspNetCore.Diagnostics.HealthChecks;
using Persistence;
using Publisher;
using Serilog;

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
            .AddControllers(options => options.Configure())
            .AddApplicationPart(Presentation.AssemblyReference.Assembly)
            .ConfigureApiBehaviorOptions(options => options.Configure())
            .AddJsonOptions(options => options.Configure());

        /* Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle */
        builder.Services.AddEndpointsApiExplorer(); //
        builder.Services.AddSwaggerGen(options => options.Configure());

        builder.Services.AddApplication();

        builder.Services.AddPersistence(builder.Configuration);

        builder.Services.AddOutboxMessagePublisher();

        builder.Services.AddHealthChecks()
            .AddSqlConnectionHealthCheck();

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
            app.UseSwaggerUI(options => options.Configure());
        }

        app.UseExceptionHandler("/error");

        app.UseHsts();

        app.UseHttpsRedirection();

        app.UseAuthorization();

        app.MapHealthChecks("/hc", new HealthCheckOptions
        {
            Predicate = _ => true,
            ResponseWriter = UIResponseWriter.WriteHealthCheckUIResponse
        });

        app.MapControllers();
    }
}


