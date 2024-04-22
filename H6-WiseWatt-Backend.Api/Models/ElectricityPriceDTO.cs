namespace H6_WiseWatt_Backend.Api.Models
{
    /// <summary>
    /// Data Transfer Object (DTO) designed to represent electricity pricing information within an API context. 
    /// This class encapsulates key details about electricity costs, 
    /// allowing structured data transfer between client applications and backend services.
    /// </summary>
    public class ElectricityPriceDTO
    {
        public DateTime TimeStamp { get; set; }
        public double PricePerKwh { get; set; }
        public double TransportAndDuties { get; set; }
        public double TotalPrice { get; set; }
    }

}
