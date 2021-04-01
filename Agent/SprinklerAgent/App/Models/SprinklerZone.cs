namespace SprinklerAgent.App.Models
{
    using HardwareMaps;

    public class SprinklerZone
    {
        public string ZoneId { get; set; }
        public Pin HardwarePin { get; set; }
    }
}
