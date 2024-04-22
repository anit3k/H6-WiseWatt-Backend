namespace H6_WiseWatt_Backend.Api.Models
{
    /// <summary>
    /// Data Transfer Object (DTO) designed to represent hourly energy consumption data for a specific device or entity. 
    /// This class facilitates structured data exchange between backend services and client applications.
    /// </summary>
    public class HourlyConsumptionDTO
    {
        public string Name { get; set; }
        public List<double> Data { get; set; }
    }
}
