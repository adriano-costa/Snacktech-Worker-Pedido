namespace WorkerService.Configuration;

public class AppSettingsConfig
{
    public AwsConfig Aws { get; private set; } = new AwsConfig();

    private AppSettingsConfig(){}

    public static AppSettingsConfig LoadConfiguration()
    {
        var builder = new ConfigurationBuilder()
            .SetBasePath(Directory.GetCurrentDirectory())
            .AddJsonFile("appsettings.Development.json", optional: true)
            .AddJsonFile("appsettings.json", optional: false);

        var configuration = builder.Build();

        if (configuration == null)
        {
            throw new InvalidOperationException(
                "Failed to load configuration. Please ensure the appsettings.json file exists and contains valid WorkerService settings.");
        }

#pragma warning disable CS8601 // Possible null reference assignment.
        var AwsConfig = new AwsConfig(){
            QueueUrl = configuration["Aws:QueueUrl"],
            DlqQueueUrl = configuration["Aws:DlqQueueUrl"],
            RegionEndpoint = configuration["Aws:RegionEndpoint"],
            ServiceURL = configuration["Aws:ServiceURL"],
            AccessKey = configuration["Aws:AccessKey"],
            SecretKey = configuration["Aws:SecretKey"]
        };
#pragma warning restore CS8601 // Possible null reference assignment.

        return new AppSettingsConfig
        {
            Aws = AwsConfig
        };
    }
}

public class AwsConfig 
{
    internal string RegionEndpoint { get; set; }  = String.Empty;
    internal string AccessKey { get; set; }  = String.Empty;
    internal string SecretKey { get; set; }  = String.Empty;
    internal string ServiceURL { get; set; }  = String.Empty;
    internal string QueueUrl { get; set; } = String.Empty;
    internal string DlqQueueUrl { get; set; }  = String.Empty;
}