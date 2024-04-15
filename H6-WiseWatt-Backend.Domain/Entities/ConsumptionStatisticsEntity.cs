namespace H6_WiseWatt_Backend.Domain.Entities
{
    public class ConsumptionStatisticsEntity
    {
        public Dictionary<string, double> DailyPercentageByDevice { get; set; } = new Dictionary<string, double>();
        public Dictionary<string, List<double>> HourlyConsumptionByDevice { get; set; } = new Dictionary<string, List<double>>();
        public Dictionary<string, double> DailyConsumptionByDevice { get; set; } = new Dictionary<string, double>();
        public double TotalDailyConsumption { get; set; }
    }
}
