namespace SprinklerAgent.App
{
    using System;
    using System.Collections.Generic;
    using System.Device.Gpio;
    using System.Threading;
    using Logging;
    using Microsoft.ApplicationInsights.DataContracts;
    using Models;

    public class SprinklerManager : ISprinklerManager
    {
        private readonly GpioController _controller = new GpioController(PinNumberingScheme.Logical);
        private SprinklerZone _activeZone = null;
        private readonly IAppLogger _logger;

        public SprinklerManager(IAppLogger logger)
        {
            _logger = logger;
        }

        public void RunZone(SprinklerZone zone, TimeSpan runSpan)
        {
            this.Log(zone: zone, message: "SprinklerManager.RunZone Begin", runSpan: runSpan, activeZoneName: _activeZone?.ZoneId ?? "null");
            if (_activeZone == null
                && zone?.HardwarePin?.SoftwarePinNumber != null
                && runSpan > TimeSpan.MinValue)
            {
                try
                {
                    _activeZone = zone;
                    _controller.OpenPin(zone.HardwarePin.SoftwarePinNumber.Value, PinMode.Output);
                    this.Log(zone: zone, message: "SprinklerManager.RunZone Running", runSpan: runSpan, activeZoneName: _activeZone?.ZoneId ?? "null");
                    //deep thoughts: Stop thread sleep when we want more real time control like stopping a 
                    Thread.Sleep(runSpan);
                }
                finally
                {
                    _controller.ClosePin(zone.HardwarePin.SoftwarePinNumber.Value);
                    _activeZone = null;
                    this.Log(zone: zone, message: "SprinklerManager.RunZone End", runSpan: runSpan, activeZoneName: _activeZone?.ZoneId ?? "null");
                    Thread.Sleep(TimeSpan.FromSeconds(5));
                }
            }
            else
            {
                this.Log(zone: zone, message: "SprinklerManager.RunZone InvalidRequest", runSpan: runSpan, activeZoneName: _activeZone?.ZoneId ?? "null");
            }
        }

        public SprinklerZone GetActiveZone()
        {
            return _activeZone;
        }

        private void Log(SprinklerZone zone, string message, TimeSpan runSpan, string activeZoneName)
        {
            _logger.Log(
                message: message,
                level: SeverityLevel.Information,
                properties: new Dictionary<string, string>
                {
                    {LogLabels.ZoneRequested, zone?.ZoneId ?? "null"},
                    {LogLabels.HardwarePin, zone?.HardwarePin?.PhysicalPinNumber.ToString()},
                    {LogLabels.SoftwarePinLabel, zone?.HardwarePin?.SoftwarePinLabel},
                    {LogLabels.SoftwarePinNumber, zone?.HardwarePin?.SoftwarePinNumber?.ToString()},
                    {LogLabels.ActiveZone, _activeZone?.ZoneId ?? "null"},
                    {LogLabels.RunSpan, runSpan.ToString()}
                });
        }
    }
}

