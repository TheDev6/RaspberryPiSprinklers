namespace Jobz.WebUi.Utilities
{
    using System;
    using System.Collections.Generic;
    using Microsoft.ApplicationInsights;
    using RootContracts.BehaviorContracts.Configuration;
    using RootContracts.BehaviorContracts.Utilities;

    public class Logger : ILogger
    {
        private readonly TelemetryClient client;
        private readonly IConfig config;

        public Logger(TelemetryClient client, IConfig config)
        {
            this.client = client;
            this.config = config;
        }

        public void TrackException(Exception exception, Dictionary<string, string> properties = null)
        {
            this.client.TrackException(exception: exception, properties: properties);
        }

        public void TrackEvent(string name, Dictionary<string, string> properties = null, Dictionary<string, double> metrics = null)
        {
            this.client.TrackEvent(eventName: name, properties: properties, metrics: metrics);
        }

        public void Dispose()
        {
            //this is expected to be called by simple injector, never seen a log yet though.
            this.TrackEvent(CustomEventNames.TelemetryClientFlush);
            this.client.Flush();
        }
    }
}