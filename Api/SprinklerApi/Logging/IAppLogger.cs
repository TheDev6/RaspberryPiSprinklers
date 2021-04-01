namespace SprinklerApi.Logging
{
    using System;
    using System.Collections.Generic;
    using Microsoft.ApplicationInsights.DataContracts;

    public interface IAppLogger
    {
        void Log(string message, SeverityLevel level, Dictionary<string, string> properties = null);
        void Log(Exception exception, Dictionary<string, string> properties = null);
    }
}
