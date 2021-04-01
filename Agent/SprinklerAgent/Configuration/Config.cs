namespace SprinklerAgent.Configuration
{
    using System;
    using System.Configuration;
    using System.Diagnostics;

    public class Config : IConfig
    {
        public string WeatherApiUrl() => ConfigurationManager.AppSettings.Get("WeatherApiUrl");
        public string WeatherApiKey() => ConfigurationManager.AppSettings.Get("WeatherApiKey");
        public string WeatherApiZip() => ConfigurationManager.AppSettings.Get("WeatherApiZip");
        public string SprinklerApiUrl() => ConfigurationManager.AppSettings.Get("SprinklerApiUrl");
        public string SprinklerApiCredentials() => ConfigurationManager.AppSettings.Get("SprinklerApiCredentials");
        public int RunFrequencySeconds() => Convert.ToInt32(ConfigurationManager.AppSettings.Get("RunFrequencySeconds"));
        public string AppInsightsCorrelationId() => ConfigurationManager.AppSettings.Get("AppInsightsCorrelationId");
        public string BlobConnectionString() => ConfigurationManager.AppSettings.Get("BlobConnectionString");
        public string BlobContainerName() => ConfigurationManager.AppSettings.Get("BlobContainerName");
        public int LogLevel() => int.Parse(ConfigurationManager.AppSettings.Get("LogLevel"));

        public string AppVersion()
        {
            var assembly = System.Reflection.Assembly.GetExecutingAssembly();
            var fileVersion = FileVersionInfo.GetVersionInfo(assembly.Location);
            return fileVersion.FileVersion;
        }
    }
}
