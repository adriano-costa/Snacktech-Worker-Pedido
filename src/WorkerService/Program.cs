using WorkerService;

var builder = Host.CreateApplicationBuilder(args);

// Configure logging using Serilog
builder.Logging.UseSerilog((context, loggerBuilder) =>
{
    loggerBuilder.AddConsole();
    loggerBuilder.AddDebug();
});

builder.Services.AddHostedService<Worker>();

var app = builder.Build();

app.Run();
