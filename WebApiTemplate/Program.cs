using WebApiTemplate.Extensions;

var builder = WebApplication.CreateBuilder(args);

/* Configure services */
builder.ConfigureServices();

/* Build app */
var app = builder.Build();

/* Configure pipeline */
app.ConfigurePipeline();

/* Run application */
app.Run();