namespace SprinklerApi.Models
{
    using System.Collections.Generic;
    using Data.Tables;

    public class ScheduleModel
    {
        public List<WeeklyWaterEvent> WaterEvents { get; set; }
        public RainDelay RainDelay { get; set; }
    }
}
