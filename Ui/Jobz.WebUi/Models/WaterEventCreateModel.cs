namespace Jobz.WebUi.Models
{
    public class WaterEventCreateModel
    {
        public string LiveId { get; set; }
        public string ZoneId { get; set; }
        public string DayOfWeek { get; set; }
        public int StartHour { get; set; }
        public int StartMinute { get; set; }
        public int StartSecond { get; set; }
        public int DurationMinutes { get; set; }
        public int DurationSeconds { get; set; }
    }
}