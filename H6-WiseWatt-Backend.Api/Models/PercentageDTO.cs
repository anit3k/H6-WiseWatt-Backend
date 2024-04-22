namespace H6_WiseWatt_Backend.Api.Models
{
    /// <summary>
    /// Data Transfer Object (DTO) used to represent percentage-based information. 
    /// This class is employed where a proportion or percentage needs to be communicated.
    /// </summary>
    public class PercentageDTO
    {
        public double Value { get; set; }
        public string Name { get; set; }
    }
}
