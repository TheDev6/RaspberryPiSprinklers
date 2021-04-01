namespace SprinklerAgent.App.Models
{
    using Sprinkler_Api.Models;
    using Weather.Models;

    public class SprinklerAgentContext
    {
        public string ScheduleJsonFileName => "bup_Schedule.json";
        public ScheduleModel Schedule { get; set; }
        public WeatherApiCacheInfo WeatherData { get; set; }
        public bool IsManualRainDelay { get; set; }
        public bool IsWeatherApiRainDelay { get; set; }
        public WeeklyWaterEvent ActiveWaterEvent { get; set; }
        public SprinklerZone ZoneToRun { get; set; }
    }
}
