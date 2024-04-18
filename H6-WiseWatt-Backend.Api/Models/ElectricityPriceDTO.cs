namespace H6_WiseWatt_Backend.Api.Models
{
    public class ElectricityPriceDTO
    {
        public DateTime TimeStamp { get; set; }
        public double PricePerKwh { get; set; }
        public double TransportAndDuties { get; set; }
        public double TotalPrice { get; set; }
    }

}
