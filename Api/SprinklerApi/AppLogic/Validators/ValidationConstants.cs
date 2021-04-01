namespace SprinklerApi.AppLogic.Validators
{
    using System.Collections.Generic;

    public static class ValidationConstants
    {
        public static readonly List<string> ValidDays = new List<string> { "Monday", "Tuesday", "Wednesday", "Thursday", "Friday", "Saturday", "Sunday" };
        public static readonly List<string> ValidZones = new List<string> { "NorthGrass", "SouthGrass", "Trees", "Bushes" };
    }
}
