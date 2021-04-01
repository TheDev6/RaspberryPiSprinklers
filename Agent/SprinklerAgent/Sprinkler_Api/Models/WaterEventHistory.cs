namespace SprinklerAgent.Sprinkler_Api.Models
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

        public string Message { get; set; }
    }
}
