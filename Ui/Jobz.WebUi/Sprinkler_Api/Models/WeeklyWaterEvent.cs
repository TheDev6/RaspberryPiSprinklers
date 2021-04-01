namespace Jobz.WebUi.Sprinkler_Api.Models
{
    using System;

    public class WeeklyWaterEvent
    {
        public Guid WeeklyWaterEventUid { get; set; }
        public string ZoneName { get; set; }
        public DayOfWeek DayOfWeek { get; set; }
        public TimeSpan Start { get; set; }
        public TimeSpan End { get; set; }
    }
}
