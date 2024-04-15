namespace H6_WiseWatt_Backend.Api.Models
{
    public class DeviceDto
    {
        public string UserGuid { get; set; }
        public  string DeviceType { get; set; }
        public string DeviceName { get; set; }
        public string Serial { get; set; }
        public bool IsOn { get; set; }
        public double EnergyConsumption { get; set; }
        public TimeSpan OnTime { get; set; }
        public TimeSpan OffTime { get; set; }
        public int? Degree { get; set; }
    }
}
