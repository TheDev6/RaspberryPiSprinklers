namespace Jobz.WebUi.Models.SprinklerAgentTypes
{
    using System;
    using System.Collections.Generic;

    public class RunSchedule
    {
        public List<WeeklyWaterEvent> WaterEvents { get; set; }
        public DateTime? RainDelayExpireDate { get; set; }
    }
}