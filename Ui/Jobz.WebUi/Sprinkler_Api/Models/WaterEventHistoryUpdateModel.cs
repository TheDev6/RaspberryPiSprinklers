namespace Jobz.WebUi.Sprinkler_Api.Models
{
    using System;

    public class WaterEventHistoryUpdateModel
    {
        public Guid WaterEventHistoryUid { get; set; }
        public string ZoneName { get; set; }
        public DateTime Start { get; set; }
        public DateTime? End { get; set; }
        public string Message { get; set; }
    }
}
