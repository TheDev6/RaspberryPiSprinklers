namespace SprinklerAgent.Sprinkler_Api.Models
{
    using System.Collections.Generic;
    public class ScheduleModel
    {
        public List<WeeklyWaterEvent> WaterEvents { get; set; }
        public RainDelay RainDelay { get; set; }
    }
}
