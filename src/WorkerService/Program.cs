using WorkerService;
using WorkerService.Configuration;
using WorkerService.Handlers;
using WorkerService.Infrastructure;
using Amazon.SQS;
using Amazon;
using WorkerService.Data;
using Microsoft.EntityFrameworkCore;
using WorkerService.Data.Repository;


var builder = Host.CreateApplicationBuilder(args);

// Load configuration
var config = AppSettingsConfig.LoadConfiguration(builder.Configuration);

// Configure Entity Framework Core with SQL Server (or another database provider)
builder.Services.AddDbContext<PedidoContext>(options =>
    options.UseSqlServer(builder.Configuration.GetConnectionString("DatabaseConnection")));


// Register configuration as a service
builder.Services.AddSingleton(config);
builder.Services.AddScoped<IPedidoRepository, PedidoRepository>();
builder.Services.AddScoped<IPedidoHandler, PedidoHandler>();
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
