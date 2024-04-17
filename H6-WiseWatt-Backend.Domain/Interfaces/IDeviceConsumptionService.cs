using H6_WiseWatt_Backend.Domain.Entities.IotEntities;

namespace H6_WiseWatt_Backend.Domain.Interfaces
{
    public interface IDeviceConsumptionService
    {
        Dictionary<string, double> GetDailyPercentageByDevice(List<IoTDeviceBaseEntity> devices);
        Dictionary<string, List<double>> GetHourlyConsumptionByDevice(List<IoTDeviceBaseEntity> devices);
        List<Tuple<string, double, double>> GetSummaryOfDailyConsumption(List<IoTDeviceBaseEntity> devices);
    }
}
