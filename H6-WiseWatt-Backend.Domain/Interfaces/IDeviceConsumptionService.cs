using H6_WiseWatt_Backend.Domain.Entities.IotEntities;

namespace H6_WiseWatt_Backend.Domain.Interfaces
{
    public interface IDeviceConsumptionService
    {
        Dictionary<string, double> GetDailyPercentageByDevice(List<IoTDeviceBaseEntity> devices);
        Dictionary<string, List<double>> GetHourlyConsumptionByDevice(List<IoTDeviceBaseEntity> devices);
        Dictionary<string, double> GetSummaryOfDailyConsumption(List<IoTDeviceBaseEntity> devices);
    }
}
