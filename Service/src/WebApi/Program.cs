using Serilog;
using WebApi.Extensions;

try
{
    var builder = WebApplication.CreateBuilder(args);

    /* Configure services */
    builder.ConfigureServices();

    /* Build app */
    var app = builder.Build();

    /* Configure pipeline */
    app.ConfigurePipeline();

    /* Run application */
    Log.Information("Service Started.");
    app.Run();
}
catch (Exception exception)
{
    Log.Fatal(exception, "Fatal error on startup.");
}
finally
{
    Log.Information("Shut down complete");
    Log.CloseAndFlush();
}