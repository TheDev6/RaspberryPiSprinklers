namespace SprinklerApi.Logging
{
    using System;
    using System.Collections.Generic;
    using Configuration;
    using Microsoft.ApplicationInsights;
    using Microsoft.ApplicationInsights.DataContracts;
    using Microsoft.ApplicationInsights.Extensibility;
    using Newtonsoft.Json;

    public class AppLogger : IAppLogger
    {
        private readonly TelemetryClient _client;
        private readonly IConfig _config;
        public AppLogger(IConfig config)
        {
            _config = config;
            _client = new TelemetryClient(new TelemetryConfiguration
            { InstrumentationKey = _config.AppInsightsCorrelationId() })
            {
                InstrumentationKey = _config.AppInsightsCorrelationId()
            };
        }

        public void Log(string message, SeverityLevel level, Dictionary<string, string> properties = null)
        {
            var enrichedProperties = Enrich(properties);
            _client.TrackTrace(message: message, severityLevel: level, properties: enrichedProperties);
            //LogToConsole(message, level, enrichedProperties);

        }

        public void Log(Exception exception, Dictionary<string, string> properties = null)
        {
            var enrichedProperties = Enrich(properties);
            _client.TrackException(exception: exception, properties: enrichedProperties);
            //LogToConsole(exception, enrichedProperties);
        }

        private void LogToConsole(string message, SeverityLevel level, Dictionary<string, string> props)
        {
            Console.WriteLine(JsonConvert.SerializeObject(new
            {
                Message = message,
                Level = level,
                Properties = props
            }, Formatting.Indented));
        }

        private void LogToConsole(Exception exception, Dictionary<string, string> props)
        {
            Console.WriteLine(JsonConvert.SerializeObject(new
            {
                Exception = exception,
                Level = SeverityLevel.Error,
                Properties = props
            }, Formatting.Indented));
        }

        private Dictionary<string, string> Enrich(Dictionary<string, string> properties)
        {
            var result = properties ?? new Dictionary<string, string>();
            if (!result.ContainsKey(LogLabels.AppVersion))
            {
                result.Add(LogLabels.AppVersion, _config.AppVersion());
            }
            if (!result.ContainsKey(LogLabels.LogTimeStamp))
            {
                result.Add(LogLabels.LogTimeStamp, DateTime.Now.ToString("O"));//iso 8601
            }
            return result;
        }
    }
}

