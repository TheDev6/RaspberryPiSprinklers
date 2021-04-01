namespace SprinklerAgent.App.Models
{
    using System.Collections.Generic;
    using HardwareMaps;

    public static class SprinklerZones
    {
        public static SprinklerZone NorthGrass = new SprinklerZone
        {
            ZoneId = ZoneLabels.NorthGrass,
            HardwarePin = PinMap.P32_Gpio12
        };

        public static SprinklerZone SouthGrass = new SprinklerZone
        {
            ZoneId = ZoneLabels.SouthGrass,
            HardwarePin = PinMap.P36_Gpio16
        };

        public static SprinklerZone Trees = new SprinklerZone
        {
            ZoneId = ZoneLabels.Trees,
            HardwarePin = PinMap.P38_Gpio20
        };

        public static SprinklerZone Bushes = new SprinklerZone
        {
            ZoneId = ZoneLabels.Bushes,
            HardwarePin = PinMap.P40_Gpio21
        };

        public static List<SprinklerZone> AsList()
        {
            return new List<SprinklerZone>
            {
                NorthGrass,
                SouthGrass,
                Trees,
                Bushes
            };
        }
    }
}
