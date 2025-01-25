using WorkerService;
using Microsoft.Extensions.Logging;
using Serilog;
using Serilog.Extensions.Hosting;
using WorkerService.Configuration;
using WorkerService.Domain;
using WorkerService.Handlers;
using WorkerService.Infrastructure;
using Amazon.SQS;
using Amazon;
using Amazon.Runtime;

var builder = Host.CreateApplicationBuilder(args);

// Load configuration
var config = AppSettingsConfig.LoadConfiguration();

// Configure logging using Serilog
Log.Logger = new LoggerConfiguration()
    .ReadFrom.Configuration(builder.Configuration)
    .CreateLogger();

builder.Logging.ClearProviders();
builder.Logging.AddSerilog();

// Register configuration as a service
builder.Services.AddSingleton(config);
builder.Services.AddSingleton<IPedidoHandler, PedidoHandler>();
builder.Services.AddSingleton<ISqsClient, SqsClient>();

// Add AWS SDK Configuration
builder.Services.AddSingleton<IAmazonSQS>(serviceProvider =>
{
    var configSqs = new AmazonSQSConfig { 
        RegionEndpoint = RegionEndpoint.GetBySystemName(config.Aws.RegionEndpoint),
        ServiceURL = config.Aws.ServiceURL,
        UseHttp = true
    };
    return new AmazonSQSClient(config.Aws.AccessKey, config.Aws.SecretKey, configSqs);
});


builder.Services.AddHostedService<Worker>();

var app = builder.Build();

app.Run();
