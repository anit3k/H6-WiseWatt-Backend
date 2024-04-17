namespace H6_WiseWatt_Backend.Api.Models
{
    public class HourlyConsumptionDto
    {
        public string Name { get; set; }
        public List<double> Data { get; set; }
    }
}
