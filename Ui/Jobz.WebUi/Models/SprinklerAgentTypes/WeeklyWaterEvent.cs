namespace Jobz.WebUi.Models.SprinklerAgentTypes
{
    using System;

    public class WeeklyWaterEvent
    {
        public string ZoneId { get; set; }
        public DayOfWeek DayOfWeek { get; set; }
        public TimeSpan Start { get; set; }
        public TimeSpan End { get; set; }
    }
}