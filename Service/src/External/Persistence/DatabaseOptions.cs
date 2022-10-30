using Microsoft.Extensions.Configuration;

namespace Persistence;

internal class DatabaseSettings
{
    public static string ConfigurationName = nameof(DatabaseSettings);
    private static IConfiguration? _configuration;
    private static DatabaseSettings? _singleton;
    private static readonly object Mutex = new();


    public string ConnectionString { get; set; } = string.Empty;
    public int MaxRetryCount { get; set; }
    public int CommandTimeOut { get; set; }
    public bool EnableDetailedErrors { get; set; }
    public bool EnableSensitiveDataLogging { get; set; }


    public static void SetConfiguration(IConfiguration config)
    {
        _configuration = config;
    }

    public static DatabaseSettings GetInstance()
    {
        if (_configuration is null)
            throw new ArgumentNullException(nameof(_configuration));

        lock (Mutex)
        {
            _singleton ??= _configuration
                .GetSection(ConfigurationName)
                .Get<DatabaseSettings>();
        }

        return _singleton;
    }
}