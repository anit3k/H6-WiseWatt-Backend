namespace H6_WiseWatt_Backend.Domain.Entities
{
    public class ElectricityPriceEntity
    {
        public DateTime TimeStamp { get; set; }
        public double PricePerKwh { get; set; }
        public double TransportAndDuties { get; set; }
        public double TotalPrice { get; set; }
    }
}
