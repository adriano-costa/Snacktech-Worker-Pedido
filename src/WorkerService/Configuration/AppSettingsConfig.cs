namespace WorkerService.Configuration;

public class AppSettingsConfig
{
    public string QueueUrl { get; private set; } = String.Empty;
    public string DlqQueueUrl { get; private set; }  = String.Empty;

    private AppSettingsConfig(){}

    public static AppSettingsConfig LoadConfiguration()
    {
        var builder = new ConfigurationBuilder()
            .SetBasePath(Directory.GetCurrentDirectory())
            .AddJsonFile("appsettings.json", optional: true);

        var configuration = builder.Build();

        if (configuration == null)
        {
            throw new InvalidOperationException(
                "Failed to load configuration. Please ensure the appsettings.json file exists and contains valid WorkerService settings.");
        }

#pragma warning disable CS8601 // Possible null reference assignment.
        return new AppSettingsConfig
        {
            QueueUrl = configuration["WorkerService:QueueUrl"],
            DlqQueueUrl = configuration["WorkerService:DlqQueueUrl"]
        };
#pragma warning restore CS8601 // Possible null reference assignment.
    }
}