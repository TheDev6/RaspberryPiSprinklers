namespace SprinklerAgent.Sprinkler_Api.Models
{
    using System;

    public class WeeklyWaterEventUpdateModel
    {
        public Guid WeeklyWaterEventUid { get; set; }
        public string ZoneName { get; set; }
        public string DayOfWeek { get; set; }
        public int StartHour { get; set; }
        public int StartMinute { get; set; }
        public int StartSecond { get; set; }
        public int DurationMinutes { get; set; }
        public int DurationSeconds { get; set; }
    }
}
