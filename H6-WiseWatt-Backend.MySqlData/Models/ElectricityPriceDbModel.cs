namespace H6_WiseWatt_Backend.MySqlData.Models
{
    public class ElectricityPriceDbModel
    {
        public int Id { get; set; }
        public DateTime TimeStamp { get; set; }
        public double PricePerKwh { get; set; }
        public double TransportAndDuties { get; set; }
        public double TotalPrice { get; set; }
    }
}
