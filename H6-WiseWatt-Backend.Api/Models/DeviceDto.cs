namespace H6_WiseWatt_Backend.Api.Models
{
    /// <summary>
    /// Data Transfer Object (DTO) designed to represent the structure and attributes of an IoT device within an API context. 
    /// DTOs are commonly used to transfer data between client applications and backend services, providing a clear contract for data exchange.
    /// </summary>
    public class DeviceDTO
    {
        public string UserGuid { get; set; }
        public  string DeviceType { get; set; }
        public string DeviceName { get; set; }
        public string Serial { get; set; }
        public bool IsManuallyOperated { get; set; }
        public bool IsOn { get; set; }
        public double EnergyConsumption { get; set; }
        public TimeSpan OnTime { get; set; }
        public TimeSpan OffTime { get; set; }
        public int? Degree { get; set; }
    }
}
