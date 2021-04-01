namespace SprinklerApi.Models
{
    public class WeeklyWaterEventCreateModel
    {
        public string ZoneName { get; set; }
        public string DayOfWeek { get; set; }
        public int StartHour { get; set; }
        public int StartMinute { get; set; }
        public int StartSecond { get; set; }
        public int DurationMinutes { get; set; }
        public int DurationSeconds { get; set; }
    }
}
