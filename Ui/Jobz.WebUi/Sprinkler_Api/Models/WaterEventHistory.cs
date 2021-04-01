namespace Jobz.WebUi.Sprinkler_Api.Models
{
    using System;

    public class WaterEventHistory
    {
        public Guid WaterEventHistoryUid { get; set; }
        public string ZoneName { get; set; }
        public DateTime Start { get; set; }
        public DateTime? End { get; set; }

        public TimeSpan? Duration
        {
            get
            {
                TimeSpan? result = null;
                if (this.End != null)
                {
                    result = this.End - this.Start;
                }
                return result;
            }
        }

        public string DurationDisplay
        {
            get
            {
                string result = null;
                if (this.End != null)
                {
                    var span = this.End.Value - this.Start;
                    var minRounded = Math.Round(span.TotalMinutes, 0, MidpointRounding.ToEven);
                    result = $"{minRounded}:m {span.Seconds}:s";
                }
                return result;
            }
        }

        public string Message { get; set; }
    }
}
