namespace H6_WiseWatt_Backend.Api.Models
{
    public class DashboardDto
    {
        public Statistics Stats { get; set; } = new Statistics();
        public List<DeviceDto> Devices { get; set; } = new List<DeviceDto>();
    }

    public class Statistics
    {
        public Dictionary<string, double> DailyPercentageByDevice { get; set; } = new Dictionary<string, double>();
        public Dictionary<string, List<double>> HourlyConsumptionByDevice { get; set; } = new Dictionary<string, List<double>>();
        public Dictionary<string, double> DailyConsumptionByDevice { get; set; } = new Dictionary<string, double>();
        public double TotalDailyConsumption { get; set; } = new double();
    }
}
