namespace H6_WiseWatt_Backend.Api.Models
{
    public class DeviceDto
    {
        public string DeviceName { get; set; }
        public double PowerConsumptionPerHour { get; set; }
        public bool IsOn { get; set; }
        public string SerialNumber { get; set; }
        public string Type { get; set; }
    }
}
