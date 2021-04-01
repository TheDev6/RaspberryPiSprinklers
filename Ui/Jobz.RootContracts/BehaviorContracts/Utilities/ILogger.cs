namespace Jobz.RootContracts.BehaviorContracts.Utilities
{
    using System;
    using System.Collections.Generic;

    public interface ILogger : IDisposable
    {
        void TrackException(Exception exception, Dictionary<string, string> properties = null);
        void TrackEvent(string name, Dictionary<string, string> properties = null, Dictionary<string, double> metrics = null);
    }
}
