namespace SprinklerApi.Models
{
    using System;

    public class WaterEventHistoryCreateModel
    {
        public string ZoneName { get; set; }
        public DateTime Start { get; set; }
        public DateTime? End { get; set; }
        public string Message { get; set; }
    }
}
