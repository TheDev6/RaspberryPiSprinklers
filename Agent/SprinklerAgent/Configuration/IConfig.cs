namespace SprinklerAgent.Configuration
{
    public interface IConfig
    {
        string WeatherApiUrl();
        string WeatherApiKey();
        string WeatherApiZip();
        string SprinklerApiUrl();
        string SprinklerApiCredentials();
        int RunFrequencySeconds();
        string AppInsightsCorrelationId();
        string BlobConnectionString();
        string BlobContainerName();
        string AppVersion();
        int LogLevel();
    }
}