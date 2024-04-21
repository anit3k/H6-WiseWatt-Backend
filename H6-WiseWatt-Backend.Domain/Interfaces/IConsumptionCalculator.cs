using H6_WiseWatt_Backend.Domain.Entities.IotEntities;

namespace H6_WiseWatt_Backend.Domain.Interfaces
{
    public interface IConsumptionCalculator
    {
        Task<Dictionary<string, double>> GetDailyPercentageByDevice(string userGuid);
        Task<Dictionary<string, List<double>>> GetHourlyConsumptionByDevice(string userGuid);
        Task<List<Tuple<string, double, double>>> GetSummaryOfDailyConsumption(string userGuid);
    }
}
