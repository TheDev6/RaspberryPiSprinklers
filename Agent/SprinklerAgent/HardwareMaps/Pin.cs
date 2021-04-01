namespace SprinklerAgent.HardwareMaps
{
    public class Pin
    {
        public int PhysicalPinNumber { get; set; }
        public PinType PinType { get; set; }
        public int? SoftwarePinNumber { get; set; }
        public string SoftwarePinLabel { get; set; }
    }
}
