namespace Jobz.WebUi.Models
{
    using System;

    public class WeeklyWaterEventModel
    {
        public Guid WeeklyWaterEventUid { get; set; }
        public string ZoneName { get; set; }
        public string DayOfWeek { get; set; }
        public int StartHour => this.StartSpan.Hours;
        public int StartMinute => this.StartSpan.Minutes;
        public int StartSecond => this.StartSpan.Seconds;
        public TimeSpan StartSpan { get; set; }
        public TimeSpan EndSpan { get; set; }
        public TimeSpan DurationSpan => this.EndSpan - this.StartSpan;
        public double DurationMinutes => Math.Round(this.DurationSpan.TotalMinutes, 0, MidpointRounding.ToEven);
        public int DurationSeconds => this.DurationSpan.Seconds;
    }
}